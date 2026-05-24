using System;
using System.Diagnostics;
using System.Numerics;
using PanelWork.Components;
using PanelWork.Entities;
using PanelWork.Primitives;

namespace PanelWork.Layouting;

public sealed class LayoutEngine(App app) {
    readonly ComponentLookup<LayoutComponent> layoutLookup = app.EntityManager.GetLookup<LayoutComponent>();

    LayoutUnit[] units = [];

    long time = 0;

    int frames = 0;

    public void Update(Entity entity) {
        long t1 = Stopwatch.GetTimestamp();

        LayoutComponent layout = layoutLookup.Get(entity);

        int index = 0;

        Index(entity, ref index);

        index = 0;

        UpdateMinSize(LayoutDirection.Horizontal, ref index);

        index = 0;

        UpdateMinSize(LayoutDirection.Vertical, ref index);

        index = 0;

        UpdateSize(LayoutDirection.Horizontal, ref index);

        index = 0;

        UpdateSize(LayoutDirection.Vertical, ref index);

        layout.LayoutBox.X = 0;
        layout.LayoutBox.Y = 0;

        index = 0;

        UpdatePos(ref index);

        long t2 = Stopwatch.GetTimestamp();

        time += t2 - t1;

        if(++frames >= 10000) {
            Console.WriteLine(time * 10000d / Stopwatch.Frequency);

            frames = 0;
            time = 0;
        }
    }

    void Index(Entity entity, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = layoutLookup.Get(entity);

        foreach(Entity child in layout.Children)
            Index(child, ref index);

        if(units.Length <= index) {
            int size = (int)BitOperations.RoundUpToPowerOf2((uint)index);

            Array.Resize(ref units, size);
        }

        units[entityIndex].Layout = layout;
    }

    void UpdateMinSize(LayoutDirection dir, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = units[entityIndex].Layout;

        int size = 0;

        for(int i = 0; i < layout.Children.Count; i++) {
            int childIndex = index;

            UpdateMinSize(dir, ref index);

            int min = dir.Min(ref units[childIndex]);

            dir.SumOrMax(layout.Layout, min, ref size);
        }

        if(dir.Is(layout.Layout))
            size += layout.Gap * (layout.Children.Count - 1);

        size += dir.Size(layout.Padding);

        int totalSize = Math.Max(dir.Min(layout), size);

        dir.Min(ref units[entityIndex]) = totalSize;
        dir.LayoutSize(layout) = totalSize;

        dir.Available(ref units[entityIndex]) = totalSize - size;
    }

    void UpdateSize(LayoutDirection dir, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = units[entityIndex].Layout;

        int available = dir.Available(ref units[entityIndex]);

        if(available <= 0)
            return;

        double stars = 0;

        for(int i = 0; i < layout.Children.Count; i++) {
            int childIndex = index + i;

            LayoutComponent childLayout = units[childIndex].Layout;

            Length length = dir.Size(childLayout);

            if(length.Unit != LengthUnit.Star)
                continue;

            available += dir.Min(ref units[childIndex]);

            stars += length.Value;
        }

        if(stars <= 0)
            return;

        int passAvailable = available;

        double pixelsPerStar = 0;

        while(true) {
            double nextPixelsPerStar = passAvailable / stars;

            if(nextPixelsPerStar == pixelsPerStar)
                break;

            pixelsPerStar = nextPixelsPerStar;

            passAvailable = available;

            stars = 0;

            for(int i = 0; i < layout.Children.Count; i++) {
                int childIndex = index + i;

                LayoutComponent childLayout = units[childIndex].Layout;

                Length length = dir.Size(childLayout);

                if(length.Unit != LengthUnit.Star)
                    continue;

                int size = (int)(length.Value * pixelsPerStar);

                int min = dir.Min(ref units[childIndex]);

                int max = dir.Max(childLayout);

                if(size <= min || size >= max) {
                    int clampedSize = Math.Max(min, Math.Min(max, size));

                    passAvailable -= clampedSize;

                    dir.LayoutSize(childLayout) = clampedSize;

                    continue;
                }

                dir.LayoutSize(childLayout) = size;

                stars += length.Value;
            }

            if(passAvailable <= 0)
                break;

            if(stars <= 0)
                break;
        }

        for(int i = 0; i < layout.Children.Count; i++)
            UpdateSize(dir, ref index);
    }

    void UpdatePos(ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = units[entityIndex].Layout;

        int x = layout.LayoutBox.X + layout.Padding.Left;
        int y = layout.LayoutBox.Y + layout.Padding.Top;

        for(int i = 0; i < layout.Children.Count; i++) {
            LayoutComponent childLayout = units[index].Layout;

            childLayout.LayoutBox.X = x;
            childLayout.LayoutBox.Y = y;

            if(layout.Layout != LayoutType.Vertical)
                x += childLayout.LayoutBox.Width + layout.Gap;
            else
                y += childLayout.LayoutBox.Height + layout.Gap;

            UpdatePos(ref index);
        }
    }
}
