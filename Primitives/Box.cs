namespace PanelWork.Primitives;

public struct Box {
    public int X;

    public int Y;

    public int Width;

    public int Height;

    public readonly bool Contains(int x, int y) {
        return x >= X && x < X + Width && y >= Y && y < Y + Height;
    }
}
