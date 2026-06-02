namespace PanelWork.Entities;

public static class ComponentRegistry {
    static int index;

    static int softCount = 4;

    public static int Count => index;

    public static int SoftCount => softCount;

    public static int GetIndex<T>() where T : IComponent {
        return Registry<T>.Index;
    }

    static int Register() {
        if(index >= softCount)
            softCount *= 2;

        return index++;
    }

    static class Registry<T> {
        public static readonly int Index;

        static Registry() {
            Index = Register();
        }
    }
}
