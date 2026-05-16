namespace PanelWork.Entities;

public static class ComponentRegistry {
    static int index;

    public static int Count => index;

    public static int GetIndex<T>() where T : IComponent {
        return Registry<T>.Index;
    }

    static class Registry<T> {
        public static readonly int Index;

        static Registry() {
            Index = index++;
        }
    }
}
