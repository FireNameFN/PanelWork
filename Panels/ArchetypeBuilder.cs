using System.Collections.Generic;
using System.Linq;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class ArchetypeBuilder(PanelManager panelManager) {
    readonly PanelManager panelManager = panelManager;

    readonly List<IConstructor> constructors = [];

    readonly List<IConstructor> destructors = [];

    readonly List<(int Event, IEventHandler Handler)> events = [];

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
        int index = panelManager.EntityManager.EnsureFactory<EventComponent<TEvent>>();

        IEventHandler handler = panelManager.GetHandler<TEventHandler, TEvent>();

        events.Add((index, handler));

        return this;
    }

    public void Clear() {
        constructors.Clear();
        destructors.Clear();

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
            Constructors = [..constructors.Distinct()],
            Destructors = [..destructors.Distinct()],
            Events = events
        };
    }
}
