using PanelWork.Components;
using PanelWork.Panels;

namespace PanelWork.Interactions;

public sealed class FocusHandler : IEventHandler<InteractionEvent> {
    public void Handle(Panel panel, ref InteractionEvent e) {
        LayoutComponent layout = panel.Get<LayoutComponent>();

        FocusComponent focus = panel.Get<FocusComponent>();

        FocusEvent focusEvent = new();

        bool emit = false;

        bool hovered = layout.LayoutBox.Contains(e.MouseX, e.MouseY);

        if(focus.Hovered != hovered) {
            focus.Hovered = hovered;

            emit = true;

            if(hovered)
                focusEvent.Entered = true;
            else
                focusEvent.Leaved = true;
        }

        bool pressed = e.MouseDown && (hovered || focus.Pressed);

        if(focus.Pressed != pressed) {
            focus.Pressed = pressed;

            emit = true;

            if(pressed)
                focusEvent.Pressed = true;
            else
                focusEvent.Released = true;
        }

        if(emit) {
            panel.Emit(ref focusEvent);

            if(focusEvent.Released && hovered)
                panel.EmitEmpty<ClickedEvent>();
        }
    }
}
