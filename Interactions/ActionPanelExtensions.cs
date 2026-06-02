using System;
using System.Threading.Tasks;

namespace PanelWork.Interactions;

public static class ActionPanelExtensions {
    extension(Panel panel) {
        public Panel Action<T>(Func<T, Task> action) {
            ActionComponent<T> button = panel.Get<ActionComponent<T>>();

            button.Action = action;

            return panel;
        }
    }
}
