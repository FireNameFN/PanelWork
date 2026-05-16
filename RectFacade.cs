using PanelWork.Layouting;
using PanelWork.Primitives;

namespace PanelWork;

public sealed class RectFacade : IFacade {
    public Color Color { get; set; }

    public void Draw(Graphics graphics, LayoutUnit unit) {
        graphics.DrawRect(unit.X, unit.Y, unit.Width, unit.Height, Color);
    }
}
