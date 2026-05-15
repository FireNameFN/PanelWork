namespace PanelWork;

public readonly struct Side(int left, int right, int top, int bottom) {
    public int Left { get; init; } = left;

    public int Right { get; init; } = right;

    public int Top { get; init; } = top;

    public int Bottom { get; init; } = bottom;

    public static implicit operator Side(int value) {
        return new(value, value, value, value);
    }
}
