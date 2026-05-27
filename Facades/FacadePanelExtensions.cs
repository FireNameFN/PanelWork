using PanelWork.Facades;
using PanelWork.Primitives;

namespace PanelWork.Components;

public static class FacadePanelExtensions {
    extension(Panel panel) {
        public Panel RectColor(Color color) {
            RectFacadeComponent facade = panel.Ensure<RectFacadeComponent>();

            facade.Color = color;

            return panel;
        }
    }
}
