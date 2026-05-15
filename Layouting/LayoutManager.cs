using System;
using System.Numerics;
using PanelWork.Entities;

namespace PanelWork.Layouting;

public sealed class LayoutManager(App app) {
    readonly App app = app;

    LayoutUnit[] units = [];

    public ReadOnlySpan<LayoutUnit> Update(Entity entity) {
        int count = 0;

        Index(entity, ref count);

        int index = 0;

        UpdateMinSize(LayoutDirection.Horizontal, entity, ref index);

        index = 0;

        UpdateMinSize(LayoutDirection.Vertical, entity, ref index);

        index = 0;

        UpdatePos(entity, ref index);

        return units.AsSpan(0, count);
    }

    void Index(Entity entity, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = app.entityManager.GetComponent<LayoutComponent>(entity);

        foreach(Entity child in layout.Children)
            Index(child, ref index);

        if(units.Length < index) {
            int size = (int)BitOperations.RoundUpToPowerOf2((uint)index);

            Array.Resize(ref units, size);
        }

        units[entityIndex].Entity = entity;
    }

    void UpdateMinSize(LayoutDirection dir, Entity entity, ref int index) {
        int entityIndex = index++;

        LayoutComponent layout = app.entityManager.GetComponent<LayoutComponent>(entity);

        int size = 0;

        for(int i = 0; i < layout.Children.Count; i++) {
            Entity child = layout.Children[i];

            int childIndex = index;

            UpdateMinSize(dir, child, ref index);

            dir.SumOrMax(units[childIndex], layout.Layout, ref size);
        }

        if(dir.Is(layout.Layout))
            size += layout.Gap * (layout.Children.Count - 1);

        size += dir.Size(layout.Padding);

        dir.Size(ref units[entityIndex]) = Math.Max(dir.Min(layout), size);
    }

    void UpdatePos(Entity entity, ref int index) {
        LayoutComponent layout = app.entityManager.GetComponent<LayoutComponent>(entity);

        int x = layout.Padding.Left;
        int y = layout.Padding.Top;

        for(int i = 0; i < layout.Children.Count; i++) {
            Entity child = layout.Children[i];

            index++;

            units[index].X = x;
            units[index].Y = y;

            if(layout.Layout != LayoutType.Vertical)
                x += units[index].Width + layout.Gap;
            else
                y += units[index].Height + layout.Gap;

            UpdatePos(child, ref index);
        }
    }
}
