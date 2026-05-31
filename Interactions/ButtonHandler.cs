using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using PanelWork.Components;
using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Interactions;

public sealed class ButtonHandler : IEventHandler<InteractionEvent> {
    PanelManager panelManager;

    ComponentLookup<LayoutComponent> layoutLookup;

    ComponentLookup<FocusComponent> focusLookup;

    public void Initialize(PanelManager panelManager) {
        this.panelManager = panelManager;

        layoutLookup = panelManager.EntityManager.GetLookup<LayoutComponent>();

        focusLookup = panelManager.EntityManager.GetLookup<FocusComponent>();
    }

    public unsafe void Handle(Entity entity, ref InteractionEvent e) {
        LayoutComponent layout = layoutLookup.Get(entity);

        FocusComponent focus = focusLookup.Get(entity);

        bool hovered = layout.LayoutBox.Contains(e.MouseX, e.MouseY);

        if(focus.Hovered != hovered) {
            if(hovered)
                panelManager.Emit(entity, ref Unsafe.NullRef<MouseEnteredEvent>());
            else
                panelManager.Emit(entity, ref Unsafe.NullRef<MouseLeavedEvent>());

            focus.Hovered = hovered;
        }
    }
}
