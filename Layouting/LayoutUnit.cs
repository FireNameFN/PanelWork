using System.Runtime.InteropServices;
using PanelWork.Entities;

namespace PanelWork.Layouting;

[StructLayout(LayoutKind.Explicit)]
public struct LayoutUnit {
    [FieldOffset(0)]
    public Entity Entity;

    [FieldOffset(8)]
    public int X;

    [FieldOffset(12)]
    public int Y;

    [FieldOffset(16)]
    public int Width;

    [FieldOffset(20)]
    public int Height;

    [FieldOffset(8)]
    public int AvailableWidth;

    [FieldOffset(12)]
    public int AvailableHeight;

    [FieldOffset(24)]
    public LayoutComponent Layout;
}
