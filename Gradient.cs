namespace PanelWork;

public struct Gradient {
    public Color Color1;

    public Color Color2;

    public Color Color3;

    public Color Color4;

    public static implicit operator Gradient(Color color) {
        return new() {
            Color1 = color,
            Color2 = color,
            Color3 = color,
            Color4 = color
        };
    }
}
