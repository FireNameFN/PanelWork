using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class PanelManager {
    public EntityManager EntityManager { get; } = new();

    readonly ComponentLookup<ArchetypeComponent> archetypeLookup;

    readonly ComponentLookup<LayoutComponent> layoutLookup;

    readonly Entity handlerEntity;

    public PanelManager() {
        archetypeLookup = EntityManager.GetLookup<ArchetypeComponent>();
        layoutLookup = EntityManager.GetLookup<LayoutComponent>();

        handlerEntity = EntityManager.CreateEntity();
    }

    public Panel CreatePanel() {
        return new(this, EntityManager.CreateEntity());
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
        HandlerComponent<TEvent> handler = EntityManager.EnsureComponent<HandlerComponent<TEvent>>(handlerEntity);

        if(handler.Handler is not null)
            return handler.Handler;

        handler.Handler = new TEventHandler();

        handler.Handler.Initialize(this);

        return handler.Handler;
    }

    public void Emit<T>(Entity entity, ref T e) {
        ArchetypeComponent archetype = archetypeLookup.Get(entity);

        EventComponent<T> eventHandler = EntityManager.GetComponent<EventComponent<T>>(archetype.Event);

        foreach(IEventHandler<T> handler in eventHandler.Handlers)
            handler.Handle(entity, ref e);
    }
}
