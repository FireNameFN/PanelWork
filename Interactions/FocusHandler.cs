using PanelWork.Components;
using PanelWork.Panels;

namespace PanelWork.Interactions;

public sealed class FocusHandler : IEventHandler<InteractionEvent> {
    public void Handle(Panel panel, ref InteractionEvent e) {
        LayoutComponent layout = panel.Get<LayoutComponent>();

        FocusComponent focus = panel.Get<FocusComponent>();

        bool hovered = layout.LayoutBox.Contains(e.MouseX, e.MouseY);

        if(focus.Hovered != hovered) {
            focus.Hovered = hovered;

            if(hovered)
                panel.EmitEmpty<MouseEnteredEvent>();
            else
                panel.EmitEmpty<MouseLeavedEvent>();
        }

        bool pressed = e.MouseDown && (hovered || focus.Pressed);

        if(focus.Pressed == pressed)
            return;

        focus.Pressed = pressed;

        if(pressed)
            panel.EmitEmpty<MousePressedEvent>();
        else
            panel.EmitEmpty<MouseReleasedEvent>();
    }
}
