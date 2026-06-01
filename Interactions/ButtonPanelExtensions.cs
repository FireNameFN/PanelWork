using System;
using System.Threading.Tasks;

namespace PanelWork.Interactions;

public static class ButtonPanelExtensions {
    extension(Panel panel) {
        public Panel Action(Func<Task> action) {
            ButtonComponent button = panel.Get<ButtonComponent>();

            button.Action = action;

            return panel;
        }
    }
}
