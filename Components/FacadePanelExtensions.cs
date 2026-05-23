using PanelWork.Facades;

namespace PanelWork.Components;

public static class FacadePanelExtensions {
    extension(Panel panel) {
        public Panel Facade(IFacade facade) {
            FacadeComponent layout = panel.Ensure<FacadeComponent>();

            layout.Facade = facade;

            return panel;
        }
    }
}
