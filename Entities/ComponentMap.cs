namespace PanelWork.Entities;

public sealed class ComponentMap {
    object[] components = new object[100];

    public void Set(int index, object component) {
        components[index] = component;
    }

    public object Get(int index) {
        return components[index];
    }
}
