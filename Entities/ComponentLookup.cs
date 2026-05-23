using System;

namespace PanelWork.Entities;

public readonly struct ComponentLookup<T> where T : class {
    readonly EntityManager entityManager;

    readonly ComponentMap<T> componentMap;

    internal ComponentLookup(EntityManager entityManager, ComponentMap<T> componentMap) {
        this.entityManager = entityManager;
        this.componentMap = componentMap;
    }

    public bool TryGet(Entity entity, out T component) {
        entityManager.ThrowIfDeleted(entity);

        component = componentMap.GetOrNull(entity.Id);

        return component is not null;
    }

    public T Get(Entity entity) {
        if(!TryGet(entity, out T component))
            throw new InvalidOperationException("Entity does not have component.");

        return component;
    }
}
