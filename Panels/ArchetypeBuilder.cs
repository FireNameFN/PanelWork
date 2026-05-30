using System.Collections.Generic;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class ArchetypeBuilder(PanelManager panelManager) {
    static readonly int ArchetypeIndex = ComponentRegistry.GetIndex<ArchetypeComponent>();

    readonly PanelManager panelManager = panelManager;

    readonly List<int> components = [];

    Entity eventEntity;

    public ArchetypeBuilder AddComponent<T>() where T : class, IComponent {
        components.Add(ComponentRegistry.GetIndex<T>());

        return this;
    }

    public ArchetypeBuilder AddEvent<TEventHandler, TEvent>() where TEventHandler : IEventHandler<TEvent>, new() {
        if(!eventEntity.IsValid)
            eventEntity = panelManager.EntityManager.CreateEntity();

        EventComponent<TEvent> eventComponent = panelManager.EntityManager.EnsureComponent<EventComponent<TEvent>>(eventEntity);

        eventComponent.Handlers.Add(panelManager.GetHandler<TEventHandler, TEvent>());

        return this;
    }

    public void Clear() {
        components.Clear();

        eventEntity = default;
    }

    public ArchetypeComponent Build() {
        return new() {
            Event = eventEntity,
            Components = [ArchetypeIndex, ..components]
        };
    }
}
