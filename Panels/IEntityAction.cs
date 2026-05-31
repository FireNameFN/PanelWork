using PanelWork.Entities;

namespace PanelWork.Panels;

public interface IEntityAction {
    public void Invoke(EntityManager entityManager, Entity entity);
}
