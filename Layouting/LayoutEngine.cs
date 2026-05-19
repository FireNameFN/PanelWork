using System;
using System.Diagnostics;
using PanelWork.Entities;

namespace PanelWork.Layouting;

public sealed class LayoutEngine(App app) {
    readonly ComponentLookup<LayoutComponent> layoutLookup = app.entityManager.GetLookup<LayoutComponent>();

    long time = 0;

    int frames = 0;

    public void Update(Entity entity) {
        long t1 = Stopwatch.GetTimestamp();

        LayoutComponent layout = layoutLookup.Get(entity);

        UpdateMinSize(layout, LayoutDirection.Horizontal);

        UpdateMinSize(layout, LayoutDirection.Vertical);

        UpdateSize(entity, LayoutDirection.Horizontal);

        UpdateSize(entity, LayoutDirection.Vertical);

        layout.LayoutX = 0;
        layout.LayoutY = 0;

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

            dir.LayoutSumOrMax(childLayout, layout.Layout, ref size);
        }

        if(dir.Is(layout.Layout))
            size += layout.Gap * (layout.Children.Count - 1);

        size += dir.Size(layout.Padding);

        int totalSize = Math.Max(dir.Min(layout), size);

        dir.LayoutSize(layout) = totalSize;

        dir.LayoutAvailable(layout) = totalSize - size;
    }

    void UpdateSize(Entity entity, LayoutDirection dir) {
        LayoutComponent layout = layoutLookup.Get(entity);

        int available = dir.LayoutAvailable(layout);

        foreach(Entity child in layout.Children) {
            LayoutComponent childLayout = layoutLookup.Get(child);

            if(dir.Size(childLayout).Unit != Primitives.LengthUnit.Star)
                continue;

            dir.LayoutSize(childLayout) += available;

            available = 0;
        }

        foreach(Entity child in layout.Children)
            UpdateSize(child, dir);
    }

    void UpdatePos(LayoutComponent layout) {
        int x = layout.LayoutX + layout.Padding.Left;
        int y = layout.LayoutY + layout.Padding.Top;

        foreach(Entity child in layout.Children) {
            LayoutComponent childLayout = layoutLookup.Get(child);

            childLayout.LayoutX = x;
            childLayout.LayoutY = y;

            if(layout.Layout != LayoutType.Vertical)
                x += childLayout.LayoutWidth + layout.Gap;
            else
                y += childLayout.LayoutHeight + layout.Gap;

            UpdatePos(childLayout);
        }
    }
}
