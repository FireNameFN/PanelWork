using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PanelWork.Primitives;

public struct Color {
    public float R;

    public float G;

    public float B;

    public float A;

    public static Color FromRgba(float r, float g, float b, float a) {
        Color color = default;

        color.R = MathF.Pow(r, 2.2f);
        color.G = MathF.Pow(g, 2.2f);
        color.B = MathF.Pow(b, 2.2f);
        color.A = MathF.Pow(a, 2.2f);

        return color;
    }

    public static implicit operator Color(Vector4 color) {
        return Unsafe.As<Vector4, Color>(ref color);
    }

    public static implicit operator Vector4(Color color) {
        return Unsafe.As<Color, Vector4>(ref color);
    }
}
