namespace PanelWork;

public static class ComponentRegistry {
    static int index = 0;

    public static int Count => index;

    public static int Register() {
        return index++;
    }
}
