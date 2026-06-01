using PanelWork.Panels;

namespace PanelWork.Interactions;

public sealed class ButtonHandler : IEventHandler<MousePressedEvent> {
    public void Handle(Panel panel, ref MousePressedEvent e) {
        ButtonComponent button = panel.Get<ButtonComponent>();

        App.Run(button.Action());
    }
}
