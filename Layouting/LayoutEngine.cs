using System;
using System.Diagnostics;
using PanelWork.Entities;
using PanelWork.Primitives;

namespace PanelWork.Layouting;

public sealed class LayoutEngine(App app) {
    readonly ComponentLookup<LayoutComponent> layoutLookup = app.entityManager.GetLookup<LayoutComponent>();

    long time = 0;

    int frames = 0;

    public void Update(Entity entity) {
        long t1 = Stopwatch.GetTimestamp();

        LayoutComponent layout = layoutLookup.Get(entity);

        UpdateMinSize(layout, LayoutDirection.Horizontal);

        UpdateMaxSize(layout, LayoutDirection.Horizontal);

        UpdateMinSize(layout, LayoutDirection.Vertical);

        UpdateMaxSize(layout, LayoutDirection.Vertical);

        UpdateSize(entity, LayoutDirection.Horizontal);

        UpdateSize(entity, LayoutDirection.Vertical);

        layout.LayoutBox.X = 0;
        layout.LayoutBox.Y = 0;

        UpdatePos(layout);

        long t2 = Stopwatch.GetTimestamp();

        time += t2 - t1;

        if(++frames >= 10000) {
            Console.WriteLine(time * 10000d / Stopwatch.Frequency);

            frames = 0;
            time = 0;
        }
    }

    void UpdateMinSize(LayoutComponent layout, LayoutDirection dir) {
        int size = 0;

        foreach(Entity child in layout.Children) {
            LayoutComponent childLayout = layoutLookup.Get(child);

            UpdateMinSize(childLayout, dir);

            dir.LayoutSumOrMaxMin(childLayout, layout.Layout, ref size);
        }

        if(dir.Is(layout.Layout))
            size += layout.Gap * (layout.Children.Count - 1);

        size += dir.Size(layout.Padding);

        int totalSize = Math.Max(dir.Min(layout), size);

        dir.LayoutMinSize(layout) = totalSize;
        dir.LayoutSize(layout) = totalSize;

        dir.LayoutAvailable(layout) = totalSize - size;
    }

    void UpdateMaxSize(LayoutComponent layout, LayoutDirection dir) {
        int size = 0;

        foreach(Entity child in layout.Children) {
            LayoutComponent childLayout = layoutLookup.Get(child);

            UpdateMaxSize(childLayout, dir);

            dir.LayoutSumOrMaxMax(childLayout, layout.Layout, ref size);
        }

        if(dir.Is(layout.Layout))
            size += layout.Gap * (layout.Children.Count - 1);

        size += dir.Size(layout.Padding);

        int totalSize = Math.Max(dir.LayoutMinSize(layout), Math.Max(dir.Max(layout), size));

        dir.LayoutMaxSize(layout) = totalSize;
    }

    void UpdateSize(Entity entity, LayoutDirection dir) {
        LayoutComponent layout = layoutLookup.Get(entity);

        int available = dir.LayoutAvailable(layout);

        if(available <= 0)
            return;

        double stars = 0;

        foreach(Entity child in layout.Children) {
            LayoutComponent childLayout = layoutLookup.Get(child);

            Length length = dir.Size(childLayout);

            if(length.Unit != LengthUnit.Star)
                continue;

            available += dir.LayoutMinSize(childLayout);

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

            foreach(Entity child in layout.Children) {
                LayoutComponent childLayout = layoutLookup.Get(child);

                Length length = dir.Size(childLayout);

                if(length.Unit != LengthUnit.Star)
                    continue;

                double size = length.Value * pixelsPerStar;

                if(size <= dir.LayoutMinSize(childLayout)) {
                    passAvailable -= dir.LayoutMinSize(childLayout);

                    dir.LayoutSize(childLayout) = dir.LayoutMinSize(childLayout);

                    continue;
                }

                if(size >= dir.LayoutMaxSize(childLayout)) {
                    passAvailable -= dir.LayoutMaxSize(childLayout);

                    dir.LayoutSize(childLayout) = dir.LayoutMaxSize(childLayout);

                    continue;
                }

                dir.LayoutSize(childLayout) = (int)size;

                stars += length.Value;
            }

            if(passAvailable <= 0)
                break;

            if(stars <= 0)
                break;
        }

        foreach(Entity child in layout.Children)
            UpdateSize(child, dir);
    }

    void UpdatePos(LayoutComponent layout) {
        int x = layout.LayoutBox.X + layout.Padding.Left;
        int y = layout.LayoutBox.Y + layout.Padding.Top;

        foreach(Entity child in layout.Children) {
            LayoutComponent childLayout = layoutLookup.Get(child);

            childLayout.LayoutBox.X = x;
            childLayout.LayoutBox.Y = y;

            if(layout.Layout != LayoutType.Vertical)
                x += childLayout.LayoutBox.Width + layout.Gap;
            else
                y += childLayout.LayoutBox.Height + layout.Gap;

            UpdatePos(childLayout);
        }
    }
}
