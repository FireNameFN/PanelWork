using PanelWork.Primitives;

namespace PanelWork;

public sealed class RectFacade : IFacade {
    public Color Color { get; set; }

    public void Draw(Graphics graphics, LayoutComponent layout) {
        graphics.DrawRect(layout.LayoutX, layout.LayoutY, layout.LayoutWidth, layout.LayoutHeight, Color);
    }
}
