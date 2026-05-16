using System;
using System.Numerics;

namespace PanelWork.Entities;

public sealed class EntityManager {
    int[] ids = new int[1];

    int[] generations = new int[1];

    int next;

    object[] maps = new object[1];

    public EntityManager() {
        for(int i = 0; i < ids.Length; i++)
            ids[i] = i + 1;
    }

    public Entity CreateEntity() {
        int index = next;

        if(ids.Length <= index) {
            int size = index * 2;

            ids = new int[size];

            Array.Resize(ref generations, size);

            for(int i = index; i < ids.Length; i++)
                ids[i] = i + 1;
        }

        next = ids[index];

        return new(index, generations[index]);
    }

    public void DeleteEntity(Entity entity) {
        ids[entity.Id] = next;

        next = entity.Id;

        generations[entity.Id]++;
    }

    public bool Exists(Entity entity) {
        return generations[entity.Id] == entity.Generation;
    }

    public bool Deleted(Entity entity) {
        return generations[entity.Id] != entity.Generation;
    }

    public ComponentLookup<T> GetLookup<T>() where T : class, IComponent {
        return new(this, GetOrCreateMap<T>());
    }

    public T AddComponent<T>(Entity entity) where T : class, IComponent, new() {
        T component = new();

        SetComponent(entity, component);

        return component;
    }

    public void SetComponent<T>(Entity entity, T component) where T : class, IComponent {
        ComponentMap<T> map = GetOrCreateMap<T>();

        map.Set(entity.Id, component);
    }

    public bool TryGetComponent<T>(Entity entity, out T component) where T : class, IComponent {
        component = null;

        if(maps.Length <= T.ComponentId)
            return false;

        if(Deleted(entity))
            return false;

        if(!TryGetMap(out ComponentMap<T> map))
            return false;

        component = map.Get(entity.Id);

        return component is not null;
    }

    public T GetComponent<T>(Entity entity) where T : class, IComponent {
        if(maps.Length <= T.ComponentId)
            return null;

        if(!TryGetMap(out ComponentMap<T> map))
            return null;

        return map.Get(entity.Id);
    }

    bool TryGetMap<T>(out ComponentMap<T> map) where T : class, IComponent {
        object mapObj = maps[T.ComponentId];

        map = (ComponentMap<T>)mapObj;

        return map is not null;
    }

    ComponentMap<T> GetOrCreateMap<T>() where T : class, IComponent {
        if(maps.Length <= T.ComponentId) {
            int size = (int)BitOperations.RoundUpToPowerOf2((uint)T.ComponentId + 1);

            Array.Resize(ref maps, size);
        }

        ref object mapObj = ref maps[T.ComponentId];

        if(mapObj is null) {
            ComponentMap<T> map = new();

            mapObj = map;

            return map;
        }

        return (ComponentMap<T>)mapObj;
    }
}
