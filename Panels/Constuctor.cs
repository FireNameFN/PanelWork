using PanelWork.Entities;

namespace PanelWork.Panels;

public interface IConstructor {
    public void Add(EntityManager entityManager, Entity entity);
}

public sealed class Constructor<T> : IConstructor where T : class, IComponent, new() {
    public static readonly Constructor<T> Instance = new();

    public void Add(EntityManager entityManager, Entity entity) {
        entityManager.SetComponent<T>(entity, new());
    }
}

public sealed class Destructor<T> : IConstructor where T : class, IComponent {
    public static readonly Destructor<T> Instance = new();

    public void Add(EntityManager entityManager, Entity entity) {
        entityManager.SetComponent<T>(entity, null);
    }
}
