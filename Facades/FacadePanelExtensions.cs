using PanelWork.Primitives;

namespace PanelWork.Facades;

public static class FacadePanelExtensions {
    extension(Panel panel) {
        public Panel RectColor(Color color) {
            RectFacadeComponent facade = panel.Ensure<RectFacadeComponent>();

            facade.Color = color;

            return panel;
        }
    }
}
