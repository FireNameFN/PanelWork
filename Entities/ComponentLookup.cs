namespace PanelWork.Entities;

public readonly struct ComponentLookup<T> where T : class {
    readonly EntityManager entityManager;

    readonly ComponentMap<T> componentMap;

    internal ComponentLookup(EntityManager entityManager, ComponentMap<T> componentMap) {
        this.entityManager = entityManager;
        this.componentMap = componentMap;
    }

    public T Get(Entity entity) {
        if(entityManager.Deleted(entity))
            return null;

        return componentMap.Get(entity.Id);
    }

    public bool TryGet(Entity entity, out T component) {
        if(entityManager.Deleted(entity)) {
            component = null;

            return false;
        }

        component = componentMap.Get(entity.Id);

        return component is not null;
    }
}
