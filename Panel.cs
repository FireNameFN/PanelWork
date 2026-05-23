using PanelWork.Entities;

namespace PanelWork;

public readonly struct Panel(EntityManager entityManager, Entity entity) {
    public readonly EntityManager EntityManager = entityManager;

    public readonly Entity Entity = entity;

    public Panel Fork() {
        return new(EntityManager, EntityManager.CreateEntity());
    }

    public T EnsureComponent<T>() where T : class, IComponent, new() {
        return EntityManager.EnsureComponent<T>(Entity);
    }
}
