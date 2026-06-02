using System.Collections.Generic;
using System.Linq;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class ArchetypeBuilder {
    readonly List<IEntityAction> constructors = [];

    readonly List<IEntityAction> destructors = [];

    readonly List<IEntityAction> events = [];

    public static ArchetypeBuilder Create() {
        return new();
    }

    public ArchetypeBuilder Add(ArchetypeComponent component) {
        constructors.AddRange(component.Constructors);
        destructors.AddRange(component.Destructors);

        events.AddRange(component.Events);

        return this;
    }

    public ArchetypeBuilder AddComponent<T>() where T : class, IComponent, new() {
        constructors.Add(Constructor<T>.Instance);
        destructors.Add(Destructor<T>.Instance);

        return this;
    }

    public ArchetypeBuilder AddEvent<TEventHandler, TEvent>() where TEventHandler : IEventHandler<TEvent>, new() {
        events.Add(EventModifier<TEventHandler, TEvent>.Instance);

        return this;
    }

    public void Clear() {
        constructors.Clear();
        destructors.Clear();

        events.Clear();
    }

    public ArchetypeComponent Build(PanelManager panelManager) {
        Entity eventEntity = default;

        IEntityAction[] events = [..this.events.Distinct()];

        if(events.Length > 0) {
            eventEntity = panelManager.EntityManager.CreateEntity();

            foreach(IEntityAction action in events)
                action.Invoke(panelManager.EntityManager, eventEntity);
        }

        return new() {
            Event = eventEntity,
            Constructors = [..constructors.Distinct()],
            Destructors = [..destructors.Distinct()],
            Events = events
        };
    }
}
