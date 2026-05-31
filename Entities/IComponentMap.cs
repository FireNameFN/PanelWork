namespace PanelWork.Entities;

public interface IComponentMap {
    public void Set(int index, IComponent component);

    public void Remove(int index);
}
