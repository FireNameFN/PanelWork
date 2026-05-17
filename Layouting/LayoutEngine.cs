using System;
using System.Diagnostics;
using System.Numerics;
using PanelWork.Entities;

namespace PanelWork.Layouting;

public sealed class LayoutEngine(App app) {
    readonly ComponentLookup<LayoutComponent> layoutLookup = app.entityManager.GetLookup<LayoutComponent>();

    LayoutUnit[] units = [];

    long time = 0;

    int frames = 0;

    public ReadOnlySpan<LayoutUnit> Update(Entity entity) {
        long t1 = Stopwatch.GetTimestamp();

        int count = 0;

        Index(entity, ref count);

        int index = 0;

        UpdateMinSize(LayoutDirection.Horizontal, ref index);

        index = 0;

        UpdateMinSize(LayoutDirection.Vertical, ref index);

        index = 0;

        UpdateSize(LayoutDirection.Horizontal, ref index);

        index = 0;

        UpdateSize(LayoutDirection.Vertical, ref index);

        index = 0;

        units[0].X = 0;
        units[0].Y = 0;

        UpdatePos(ref index);

        long t2 = Stopwatch.GetTimestamp();

        time += t2 - t1;

        if(++frames >= 100) {
            Console.WriteLine(time * 10000d / Stopwatch.Frequency);

            frames = 0;
            time = 0;
        }

        return units.AsSpan(0, count);
    }

    void Index(Entity entity, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = layoutLookup.Get(entity);

        foreach(Entity child in layout.Children)
            Index(child, ref index);

        if(units.Length < index) {
            int size = (int)BitOperations.RoundUpToPowerOf2((uint)index);

            Array.Resize(ref units, size);
        }

        units[entityIndex].Entity = entity;
        units[entityIndex].Layout = layout;
    }

    void UpdateMinSize(LayoutDirection dir, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = units[entityIndex].Layout;

        int size = 0;

        for(int i = 0; i < layout.Children.Count; i++) {
            int childIndex = index;

            UpdateMinSize(dir, ref index);

            dir.SumOrMax(units[childIndex], layout.Layout, ref size);
        }

        if(dir.Is(layout.Layout))
            size += layout.Gap * (layout.Children.Count - 1);

        size += dir.Size(layout.Padding);

        int totalSize = Math.Max(dir.Min(layout), size);

        dir.Size(ref units[entityIndex]) = totalSize;

        dir.Available(ref units[entityIndex]) = totalSize - size;
    }

    void UpdateSize(LayoutDirection dir, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = units[entityIndex].Layout;

        int available = dir.Available(ref units[entityIndex]);

        for(int i = 0; i < layout.Children.Count; i++) {
            ref LayoutUnit childUnit = ref units[index + i];

            LayoutComponent childLayout = childUnit.Layout;

            if(dir.Size(childLayout).Unit != Primitives.LengthUnit.Star)
                continue;

            dir.Size(ref childUnit) += available;

            available = 0;
        }

        for(int i = 0; i < layout.Children.Count; i++)
            UpdateSize(dir, ref index);
    }

    void UpdatePos(ref int index) {
        LayoutComponent layout = units[index].Layout;

        int x = units[index].X + layout.Padding.Left;
        int y = units[index].Y + layout.Padding.Top;

        for(int i = 0; i < layout.Children.Count; i++) {
            index++;

            units[index].X = x;
            units[index].Y = y;

            if(layout.Layout != LayoutType.Vertical)
                x += units[index].Width + layout.Gap;
            else
                y += units[index].Height + layout.Gap;

            UpdatePos(ref index);
        }
    }
}
