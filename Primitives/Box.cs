namespace PanelWork.Primitives;

public struct Box {
    public int X;

    public int Y;

    public int Width;

    public int Height;

    public readonly int X2 => X + Width;

    public readonly int Y2 => Y + Height;

    public readonly bool Contains(int x, int y) {
        return x >= X && x < X2 && y >= Y && y < Y2;
    }
}
