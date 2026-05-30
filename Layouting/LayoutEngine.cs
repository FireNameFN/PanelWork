using System;
using System.Diagnostics;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Layouting;

public sealed class LayoutEngine(App app) {
    readonly ComponentLookup<LayoutComponent> layoutLookup = app.PanelManager.EntityManager.GetLookup<LayoutComponent>();

    LayoutUnit[] units = new LayoutUnit[64];

    long time = 0;

    int frames = 0;

    public void Update(Entity entity, int width, int height) {
        long t1 = Stopwatch.GetTimestamp();

        LayoutComponent layout = layoutLookup.Get(entity);

        int index = 0;

        Index(entity, ref index);

        index = 0;

        UpdateMinSize(LayoutDirection.Horizontal, ref index);

        index = 0;

        UpdateMinSize(LayoutDirection.Vertical, ref index);

        layout.LayoutBox.Width = PanelMath.Clamp(width, units[0].MinWidth, layout.MaxWidth);
        layout.LayoutBox.Height = PanelMath.Clamp(height, units[0].MinHeight, layout.MaxHeight);

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

        for(int i = 0; i < layout.PanelCount; i++)
            Index(layout.Panels[i], ref index);

        if(units.Length <= index)
            Array.Resize(ref units, units.Length * 2);

        units[entityIndex].Layout = layout;
    }

    void UpdateMinSize(LayoutDirection dir, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = units[entityIndex].Layout;

        int size = 0;

        for(int i = 0; i < layout.PanelCount; i++) {
            int subpanelIndex = index;

            UpdateMinSize(dir, ref index);

            int min = dir.Min(ref units[subpanelIndex]);

            dir.SumOrMax(layout.Layout, min, ref size);
        }

        if(dir.Is(layout.Layout))
            size += layout.Gap * (layout.PanelCount - 1);

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

        for(int i = 0; i < layout.PanelCount; i++) {
            int subpanelIndex = index + i;

            LayoutComponent subpanelLayout = units[subpanelIndex].Layout;

            double star = dir.Star(subpanelLayout);

            if(star <= 0)
                continue;

            available += dir.Min(ref units[subpanelIndex]);

            stars += star;
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

            for(int i = 0; i < layout.PanelCount; i++) {
                int subpanelIndex = index + i;

                LayoutComponent subpanelLayout = units[subpanelIndex].Layout;

                double star = dir.Star(subpanelLayout);

                if(star <= 0)
                    continue;

                int size = (int)(star * pixelsPerStar);

                int min = dir.Min(ref units[subpanelIndex]);

                int max = dir.Max(subpanelLayout);

                if(size <= min || size >= max) {
                    size = PanelMath.Clamp(size, min, max);

                    passAvailable -= size;

                    dir.LayoutSize(subpanelLayout) = size;

                    continue;
                }

                dir.LayoutSize(subpanelLayout) = size;

                stars += star;
            }

            if(passAvailable <= 0)
                break;

            if(stars <= 0)
                break;
        }

        for(int i = 0; i < layout.PanelCount; i++)
            UpdateSize(dir, ref index);
    }

    void UpdatePos(ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = units[entityIndex].Layout;

        int x = layout.LayoutBox.X + layout.Padding.Left;
        int y = layout.LayoutBox.Y + layout.Padding.Top;

        for(int i = 0; i < layout.PanelCount; i++) {
            LayoutComponent subpanelLayout = units[index].Layout;

            subpanelLayout.LayoutBox.X = x;
            subpanelLayout.LayoutBox.Y = y;

            if(layout.Layout != LayoutType.Vertical)
                x += subpanelLayout.LayoutBox.Width + layout.Gap;
            else
                y += subpanelLayout.LayoutBox.Height + layout.Gap;

            UpdatePos(ref index);
        }
    }
}
