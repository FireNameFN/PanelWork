using System;
using PanelWork.Primitives;

namespace PanelWork.Layouting;

public readonly struct LayoutDirection {
    public static readonly LayoutDirection Horizontal = new(true);

    public static readonly LayoutDirection Vertical = new(false);

    public bool IsHorizontal { get; }

    public bool IsVertical => IsHorizontal;

    private LayoutDirection(bool horizontal) {
        IsHorizontal = horizontal;
    }

    public bool Is(LayoutType type) {
        return IsHorizontal == (type != LayoutType.Vertical);
    }

    public void LayoutSumOrMaxMin(LayoutComponent unit, LayoutType type, ref int value) {
        int val = LayoutMinSize(unit);

        if(Is(type))
            value += val;
        else
            value = Math.Max(value, val);
    }

    public void LayoutSumOrMaxMax(LayoutComponent unit, LayoutType type, ref int value) {
        int val = LayoutMaxSize(unit);

        if(Is(type))
            value += val;
        else
            value = Math.Max(value, val);
    }

    public ref int LayoutSize(LayoutComponent unit) {
        if(IsHorizontal)
            return ref unit.LayoutBox.Width;

        return ref unit.LayoutBox.Height;
    }

    public ref int LayoutMinSize(LayoutComponent unit) {
        if(IsHorizontal)
            return ref unit.LayoutBox.MinWidth;

        return ref unit.LayoutBox.MinHeight;
    }

    public ref int LayoutMaxSize(LayoutComponent unit) {
        if(IsHorizontal)
            return ref unit.LayoutBox.MaxWidth;

        return ref unit.LayoutBox.MaxHeight;
    }

    public ref int LayoutAvailable(LayoutComponent unit) {
        if(IsHorizontal)
            return ref unit.LayoutBox.AvailableWidth;

        return ref unit.LayoutBox.AvailableHeight;
    }

    public int Min(LayoutComponent layout) {
        return IsHorizontal ? layout.MinWidth : layout.MinHeight;
    }

    public int Max(LayoutComponent layout) {
        return IsHorizontal ? layout.MaxWidth : layout.MaxHeight;
    }

    public Length Size(LayoutComponent layout) {
        return IsHorizontal ? layout.Width : layout.Height;
    }

    public int Size(Side side) {
        if(IsHorizontal)
            return side.Left + side.Right;

        return side.Top + side.Bottom;
    }
}
