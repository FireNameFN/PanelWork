using System;

namespace PanelWork.Entities;

public readonly struct ComponentLookup<T>(EntityManager entityManager, ComponentMap<T> componentMap) where T : class {
    readonly EntityManager entityManager = entityManager;

    readonly ComponentMap<T> componentMap = componentMap;

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
