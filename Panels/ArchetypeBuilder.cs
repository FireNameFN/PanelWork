using System;
using System.Collections.Generic;
using System.Linq;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class ArchetypeBuilder(PanelManager panelManager) {
    static readonly int ArchetypeIndex = TypeRegistry<IComponent>.GetIndex<ArchetypeComponent>();

    readonly PanelManager panelManager = panelManager;

    readonly List<int> components = [];

    readonly List<(int Event, IEventHandler Handler)> events = [];

    public ArchetypeBuilder Add(ArchetypeComponent component) {
        components.AddRange(component.Components.AsSpan(1));

        events.AddRange(component.Events);

        return this;
    }

    public ArchetypeBuilder AddComponent<T>() where T : class, IComponent, new() {
        int index = panelManager.EntityManager.EnsureFactory<T>();

        components.Add(index);

        return this;
    }

    public ArchetypeBuilder AddEvent<TEventHandler, TEvent>() where TEventHandler : IEventHandler<TEvent>, new() {
        int index = panelManager.EntityManager.EnsureFactory<EventComponent<TEvent>>();

        IEventHandler handler = panelManager.GetHandler<TEventHandler, TEvent>();

        events.Add((index, handler));

        return this;
    }

    public void Clear() {
        components.Clear();

        events.Clear();
    }

    public ArchetypeComponent Build() {
        Entity eventEntity = panelManager.EntityManager.CreateEntity();

        (int Event, IEventHandler Handler)[] events = [..this.events.Distinct()];

        foreach((int e, IEventHandler handler) in events) {
            IEventComponent comp = (IEventComponent)panelManager.EntityManager.EnsureComponent(eventEntity, e);

            comp.Add(handler);
        }

        return new() {
            Event = eventEntity,
            Components = [ArchetypeIndex, ..components.Distinct()],
            Events = events
        };
    }
}
