using System.Runtime.InteropServices;

namespace PanelWork.Layouting;

[StructLayout(LayoutKind.Explicit)]
public struct LayoutBox {
    [FieldOffset(0)]
    public int X;

    [FieldOffset(4)]
    public int Y;

    [FieldOffset(0)]
    public int AvailableWidth;

    [FieldOffset(4)]
    public int AvailableHeight;

    [FieldOffset(8)]
    public int Width;

    [FieldOffset(12)]
    public int Height;
}
