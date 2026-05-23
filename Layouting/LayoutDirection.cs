using System;
using PanelWork.Components;
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

    public void SumOrMax(LayoutType type, int value, ref int result) {
        if(Is(type))
            result += value;
        else
            result = Math.Max(result, value);
    }

    public ref int Min(ref LayoutUnit unit) {
        if(IsHorizontal)
            return ref unit.MinWidth;

        return ref unit.MinHeight;
    }

    public ref int Max(ref LayoutUnit unit) {
        if(IsHorizontal)
            return ref unit.MaxWidth;

        return ref unit.MaxHeight;
    }

    public ref int Available(ref LayoutUnit unit) {
        if(IsHorizontal)
            return ref unit.AvailableWidth;

        return ref unit.AvailableHeight;
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

    public ref int LayoutSize(LayoutComponent unit) {
        if(IsHorizontal)
            return ref unit.LayoutBox.Width;

        return ref unit.LayoutBox.Height;
    }

    public int Size(Side side) {
        if(IsHorizontal)
            return side.Left + side.Right;

        return side.Top + side.Bottom;
    }
}
