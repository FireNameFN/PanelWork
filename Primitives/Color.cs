using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PanelWork.Primitives;

public struct Color {
    public float R;

    public float G;

    public float B;

    public float A;

    public static Color FromRgba(float r, float g, float b, float a = 1) {
        return new() {
            R = Gamma(r),
            G = Gamma(g),
            B = Gamma(b),
            A = a
        };
    }

    static float Gamma(float value) {
        return MathF.Pow(value, 2.2f);
    }

    public static implicit operator Color(uint argb) {
        return FromRgba((argb >> 16 & 0xFF) / 255f, (argb >> 8 & 0xFF) / 255f, (argb & 0xFF) / 255f, (argb >> 24 & 0xFF) / 255f);
    }

    public static implicit operator Color(Vector4 color) {
        return Unsafe.As<Vector4, Color>(ref color);
    }

    public static implicit operator Vector4(Color color) {
        return Unsafe.As<Color, Vector4>(ref color);
    }
}
