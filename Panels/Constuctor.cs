using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class Constructor<T> : IEntityAction where T : class, IComponent, new() {
    public static readonly Constructor<T> Instance = new();

    public void Invoke(EntityManager entityManager, Entity entity) {
        entityManager.SetComponent<T>(entity, new());
    }
}
