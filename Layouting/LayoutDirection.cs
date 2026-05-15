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

    public void SumOrMax(LayoutUnit unit, LayoutType type, ref int value) {
        int val = Size(ref unit);

        if(Is(type))
            value += val;
        else
            value = Math.Max(value, val);
    }

    public ref int Pos(ref LayoutUnit unit) {
        if(IsHorizontal)
            return ref unit.X;

        return ref unit.Y;
    }

    public ref int Size(ref LayoutUnit unit) {
        if(IsHorizontal)
            return ref unit.Width;

        return ref unit.Height;
    }

    public int Min(LayoutComponent layout) {
        return IsHorizontal ? layout.MinWidth : layout.MinHeight;
    }

    public int Size(LayoutComponent layout) {
        return IsHorizontal ? layout.Width : layout.Height;
    }

    public int Size(Side side) {
        if(IsHorizontal)
            return side.Left + side.Right;

        return side.Top + side.Bottom;
    }
}
