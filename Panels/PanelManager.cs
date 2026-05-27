using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class PanelManager {
    public EntityManager EntityManager { get; } = new();

    readonly ComponentLookup<ArchetypeComponent> archetypeLookup;

    public PanelManager() {
        archetypeLookup = EntityManager.GetLookup<ArchetypeComponent>();
    }

    public Panel CreatePanel() {
        return new(this, EntityManager.CreateEntity());
    }

    public void Emit<T>(Entity entity, ref T e) {
        ArchetypeComponent archetype = archetypeLookup.Get(entity);

        EventComponent<T> eventHandler = EntityManager.GetComponent<EventComponent<T>>(archetype.Event);

        foreach(IEventHandler<T> handler in eventHandler.Handlers)
            handler.Handle(entity, ref e);
    }
}
