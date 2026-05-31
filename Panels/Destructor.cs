using PanelWork.Entities;

namespace PanelWork.Panels;

public sealed class Destructor<T> : IEntityAction where T : class, IComponent {
    public static readonly Destructor<T> Instance = new();

    public void Invoke(EntityManager entityManager, PanelManager panelManager, Entity entity) {
        entityManager.SetComponent<T>(entity, null);
    }
}
