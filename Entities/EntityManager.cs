namespace PanelWork.Entities;

public sealed class EntityManager {
    readonly DataList<int> generations = new(100);

    ComponentMap[] maps = new ComponentMap[100];

    public EntityManager() {
        for(int i = 0; i < 100; i++)
            maps[i] = new();
    }

    public Entity CreateEntity() {
        int index = generations.Add(out int generation);

        return new(index, generation);
    }

    public void DeleteEntity(Entity entity) {
        generations.GetRef(entity.Id)++;

        generations.Remove(entity.Id);
    }

    public T AddComponent<T>(Entity entity) where T : IComponent, new() {
        T component = new();

        SetComponent(entity, component);

        return component;
    }

    public void SetComponent<T>(Entity entity, T component) where T : IComponent {
        ComponentMap map = maps[T.ComponentId];

        map.Set(entity.Id, component);
    }

    public T GetComponent<T>(Entity entity) where T : IComponent {
        ComponentMap map = maps[T.ComponentId];

        return (T)map.Get(entity.Id);
    }
}
