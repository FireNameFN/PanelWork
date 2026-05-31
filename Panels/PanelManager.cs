using System;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class PanelManager {
    public EntityManager EntityManager { get; } = new();

    public GenericArchetypes Archetypes { get; }

    ISingleton[] eventHandlers = new ISingleton[TypeRegistry<ISingleton>.SoftCount];

    readonly ComponentLookup<ArchetypeComponent> archetypeLookup;

    readonly ComponentLookup<LayoutComponent> layoutLookup;

    public PanelManager() {
        archetypeLookup = EntityManager.GetLookup<ArchetypeComponent>();
        layoutLookup = EntityManager.GetLookup<LayoutComponent>();

        Archetypes = new(this);
    }

    public Panel CreatePanel(ArchetypeComponent archetype) {
        Panel panel = new(this, EntityManager.CreateEntity());

        archetypeLookup.Set(panel.Entity, archetype);

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

        EntityManager.RemoveComponents(entity, archetype.Components);

        EntityManager.DeleteEntity(entity);
    }

    public IEventHandler<TEvent> GetHandler<TEventHandler, TEvent>() where TEventHandler : IEventHandler<TEvent>, new() {
        int index = TypeRegistry<ISingleton>.GetIndex<TEventHandler>();

        if(eventHandlers.Length <= index)
            Array.Resize(ref eventHandlers, TypeRegistry<ISingleton>.SoftCount);

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
