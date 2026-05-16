using System;
using System.Numerics;
using System.Runtime.CompilerServices;

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

            Array.Resize(ref generations, size);

            ids = new int[size];

            for(int i = index; i < size; i++)
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

        if(Deleted(entity))
            return false;

        if(!TryGetMap(out ComponentMap<T> map))
            return false;

        component = map.Get(entity.Id);

        return component is not null;
    }

    public T GetComponent<T>(Entity entity) where T : class, IComponent {
        if(!TryGetMap(out ComponentMap<T> map))
            return null;

        return map.Get(entity.Id);
    }

    bool TryGetMap<T>(out ComponentMap<T> map) where T : class, IComponent {
        Unsafe.SkipInit(out map);

        int index = ComponentRegistry.GetIndex<T>();

        if(maps.Length <= index)
            return false;

        object mapObj = maps[index];

        map = (ComponentMap<T>)mapObj;

        return map is not null;
    }

    ComponentMap<T> GetOrCreateMap<T>() where T : class, IComponent {
        int index = ComponentRegistry.GetIndex<T>();

        if(maps.Length <= index) {
            int size = (int)BitOperations.RoundUpToPowerOf2((uint)ComponentRegistry.Count);

            Array.Resize(ref maps, size);
        }

        ref object mapObj = ref maps[index];

        if(mapObj is null) {
            ComponentMap<T> map = new();

            mapObj = map;

            return map;
        }

        return (ComponentMap<T>)mapObj;
    }
}
