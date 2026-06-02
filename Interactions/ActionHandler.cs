using PanelWork.Panels;

namespace PanelWork.Interactions;

public sealed class ActionHandler<T> : IEventHandler<T> {
    public void Handle(Panel panel, ref T e) {
        ActionComponent<T> button = panel.Get<ActionComponent<T>>();

        App.Run(button.Action(e));
    }
}
