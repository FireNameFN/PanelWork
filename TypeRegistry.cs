namespace PanelWork;

public static class TypeRegistry<TType> {
    static int index;

    static int softCount = 4;

    public static int Count => index;

    public static int SoftCount => softCount;

    public static int GetIndex<T>() where T : TType {
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
