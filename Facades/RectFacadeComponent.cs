using PanelWork.Entities;
using PanelWork.Primitives;

namespace PanelWork.Facades;

public sealed class RectFacadeComponent : IComponent {
    public Color Color { get; set; }

    public static RectFacadeComponent FromColor(Color color) {
        return new() {
            Color = color
        };
    }
}
