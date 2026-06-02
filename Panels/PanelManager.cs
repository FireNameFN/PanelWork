using System;
using System.Diagnostics;
using PanelWork.Components;
using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class PanelManager {
    public EntityManager EntityManager { get; } = new();

    public GenericArchetypes Archetypes { get; }

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
            constructor.Invoke(EntityManager, panel.Entity);

        long t2 = Stopwatch.GetTimestamp();

        Console.WriteLine((t2 - t1) * 1000000d / Stopwatch.Frequency);

        return panel;
    }

    public void DeletePanel(Entity entity) {
        LayoutComponent layout = layoutLookup.Get(entity);

        for(int i = 0; i < layout.PanelCount; i++)
            DeletePanel(layout.Panels[i]);

        ArchetypeComponent archetype = archetypeLookup.Get(entity);

        archetypeLookup.Set(entity, null);

        foreach(IEntityAction constructor in archetype.Destructors)
            constructor.Invoke(EntityManager, entity);

        EntityManager.DeleteEntity(entity);
    }

    public void Emit<T>(Entity entity, ref T e) {
        ArchetypeComponent archetype = archetypeLookup.Get(entity);

        if(!archetype.Event.IsValid)
            return;

        if(!EntityManager.TryGetComponent(archetype.Event, out EventComponent<T> eventComponent))
            return;

        foreach(IEventHandler<T> handler in eventComponent.Handlers)
            handler.Handle(new(this, entity), ref e);
    }
}
