using System;
using System.Diagnostics;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class PanelManager {
    public EntityManager EntityManager { get; } = new();

    public GenericArchetypes Archetypes { get; }

    IEventHandler[] eventHandlers = new IEventHandler[TypeRegistry<IEventHandler>.SoftCount];

    readonly ComponentLookup<ArchetypeComponent> archetypeLookup;

    readonly ComponentLookup<LayoutComponent> layoutLookup;

    public PanelManager() {
        archetypeLookup = EntityManager.GetLookup<ArchetypeComponent>();
        layoutLookup = EntityManager.GetLookup<LayoutComponent>();

        Archetypes = new(this);
    }

    public Panel CreatePanel(ArchetypeComponent archetype) {
        long t1 = Stopwatch.GetTimestamp();

        Panel panel = new(this, EntityManager.CreateEntity());

        archetypeLookup.Set(panel.Entity, archetype);

        foreach(IEntityAction constructor in archetype.Constructors)
            constructor.Invoke(EntityManager, this, panel.Entity);

        long t2 = Stopwatch.GetTimestamp();

        Console.WriteLine((t2 - t1) * 1000000d / Stopwatch.Frequency);

        return panel;
    }

    public ArchetypeBuilder CreateArchetypeBuilder() {
        return new(this);
    }

    public void DeletePanel(Entity entity) {
        LayoutComponent layout = layoutLookup.Get(entity);

        for(int i = 0; i < layout.PanelCount; i++)
            DeletePanel(layout.Panels[i]);

        ArchetypeComponent archetype = archetypeLookup.Get(entity);

        archetypeLookup.Set(entity, null);

        foreach(IEntityAction constructor in archetype.Destructors)
            constructor.Invoke(EntityManager, this, entity);

        EntityManager.DeleteEntity(entity);
    }

    public IEntityAction GetConstructor<T>() where T : class, IComponent, new() {
        int index = TypeRegistry<IEventHandler>.GetIndex<Constructor<T>>();

        if(eventHandlers.Length <= index)
            Array.Resize(ref eventHandlers, TypeRegistry<IEventHandler>.SoftCount);

        if(eventHandlers[index] is not null)
            return (IEntityAction)eventHandlers[index];

        Constructor<T> eventHandler = new();

        eventHandler.Initialize(this);

        eventHandlers[index] = eventHandler;

        return eventHandler;
    }

    public IEventHandler<TEvent> GetHandler<TEventHandler, TEvent>() where TEventHandler : IEventHandler<TEvent>, new() {
        int index = TypeRegistry<IEventHandler>.GetIndex<TEventHandler>();

        if(eventHandlers.Length <= index)
            Array.Resize(ref eventHandlers, TypeRegistry<IEventHandler>.SoftCount);

        if(eventHandlers[index] is not null)
            return (IEventHandler<TEvent>)eventHandlers[index];

        TEventHandler eventHandler = new();

        eventHandler.Initialize(this);

        eventHandlers[index] = eventHandler;

        return eventHandler;
    }

    public void Emit<T>(Entity entity, ref T e) {
        ArchetypeComponent archetype = archetypeLookup.Get(entity);

        if(EntityManager.Deleted(archetype.Event))
            return;

        if(!EntityManager.TryGetComponent(archetype.Event, out EventComponent<T> eventComponent))
            return;

        foreach(IEventHandler<T> handler in eventComponent.Handlers)
            handler.Handle(entity, ref e);
    }
}
