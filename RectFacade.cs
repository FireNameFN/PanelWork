using PanelWork.Layouting;
using PanelWork.Primitives;

namespace PanelWork;

public sealed class RectFacade : IFacade {
    public Color Color { get; set; }

    public static RectFacade FromColor(Color color) {
        return new() {
            Color = color
        };
    }

    public void Draw(Graphics graphics, LayoutBox box) {
        graphics.DrawRect(box.X, box.Y, box.Width, box.Height, Color);
    }
}
