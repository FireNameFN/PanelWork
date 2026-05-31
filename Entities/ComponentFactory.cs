using System;

namespace PanelWork.Entities;

public static class ComponentFactory {
    static Func<IComponent>[] factories = new Func<IComponent>[4];

    public static int Register<T>() where T : IComponent, new() {
        int index = TypeRegistry<IComponent>.GetIndex<T>();

        if(factories.Length <= index)
            Array.Resize(ref factories, TypeRegistry<IComponent>.SoftCount);
        else if(factories[index] is not null)
            return index;

        factories[index] = static () => new T();

        return index;
    }

    public static IComponent Create(int index) {
        return factories[index]();
    }
}
