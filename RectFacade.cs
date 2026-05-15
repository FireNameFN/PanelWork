using System.Numerics;

namespace PanelWork;

public sealed class RectFacade : IFacade {
    public Vector4 Color { get; set; }

    public void Draw(Graphics graphics, LayoutUnit unit) {
        graphics.DrawRect(unit.X, unit.Y, unit.Width, unit.Height, Color);
    }
}
