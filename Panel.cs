using PanelWork.Components;
using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork;

public readonly struct Panel(PanelManager panelManager, Entity entity) {
    public readonly PanelManager PanelManager = panelManager;

    public readonly Entity Entity = entity;

    public Panel Fork(ArchetypeComponent archetype) {
        return PanelManager.CreatePanel(archetype);
    }

    public T Get<T>() where T : class, IComponent, new() {
        return PanelManager.EntityManager.GetComponent<T>(Entity);
    }

    public void Emit<T>(ref T e) {
        PanelManager.Emit(Entity, ref e);
    }

    public void EmitEmpty<T>() {
        T e = default;

        PanelManager.Emit(Entity, ref e);
    }
}
