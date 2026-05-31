using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class Constructor<T> : IEntityAction, IEventHandler where T : class, IComponent, new() {
    public static readonly Constructor<T> Instance = new();

    ComponentLookup<T> lookup;

    public void Initialize(PanelManager panelManager) {
        lookup = panelManager.EntityManager.GetLookup<T>();
    }

    public void Invoke(EntityManager entityManager, PanelManager panelManager, Entity entity) {
        entityManager.SetComponent<T>(entity, new());

        //lookup.Set(entity, new());
    }
}
