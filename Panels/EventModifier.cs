using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class EventModifier<TEventHandler, TEvent> : IEntityAction where TEventHandler : IEventHandler<TEvent>, new() {
    public static readonly EventModifier<TEventHandler, TEvent> Instance = new();

    public void Invoke(EntityManager entityManager, PanelManager panelManager, Entity entity) {
        EventComponent<TEvent> component = entityManager.EnsureComponent<EventComponent<TEvent>>(entity);

        component.Handlers.Add(panelManager.GetHandler<TEventHandler, TEvent>());
    }
}
