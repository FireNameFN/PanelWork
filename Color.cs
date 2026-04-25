using System.Numerics;
using System.Runtime.CompilerServices;

namespace PanelWork;

public struct Color {
    public float R;

    public float G;

    public float B;

    public float A;

    public static implicit operator Color(Vector4 color) {
        return Unsafe.As<Vector4, Color>(ref color);
    }

    public static implicit operator Vector4(Color color) {
        return Unsafe.As<Color, Vector4>(ref color);
    }
}
