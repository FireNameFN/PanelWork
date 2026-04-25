using System.Numerics;

namespace PanelWork;

public struct Radius {
    public Vector2 Radius1;

    public Vector2 Radius2;

    public Vector2 Radius3;

    public Vector2 Radius4;

    public Radius() { }

    public Radius(Vector2 radius1, Vector2 radius2, Vector2 radius3, Vector2 radius4) {
        Radius1 = radius1;
        Radius2 = radius2;
        Radius3 = radius3;
        Radius4 = radius4;
    }

    public static implicit operator Radius(Vector2 value) {
        return new(value, value, value, value);
    }

    public static implicit operator Radius(int value) {
        return value;
    }
}
