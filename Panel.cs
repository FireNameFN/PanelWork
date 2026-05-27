using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork;

public readonly struct Panel(PanelManager panelManager, Entity entity) {
    public readonly PanelManager PanelManager = panelManager;

    public readonly Entity Entity = entity;

    public Panel Fork() {
        return PanelManager.CreatePanel();
    }

    public T Ensure<T>() where T : class, IComponent, new() {
        return PanelManager.EntityManager.EnsureComponent<T>(Entity);
    }

    public Panel Set<T>(T component) where T : class, IComponent {
        PanelManager.EntityManager.SetComponent(Entity, component);

        return this;
    }

    public void Emit<T>(ref T e) {
        PanelManager.Emit(Entity, ref e);
    }
}
