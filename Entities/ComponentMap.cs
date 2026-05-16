using System;
using System.Numerics;

namespace PanelWork.Entities;

public sealed class ComponentMap<T> where T : class {
    T[] components = [];

    public void Set(int index, T component) {
        if(components.Length <= index) {
            int size = (int)BitOperations.RoundUpToPowerOf2((uint)index + 1);

            Array.Resize(ref components, size);
        }

        components[index] = component;
    }

    public T Get(int index) {
        if(components.Length <= index)
            return null;

        return components[index];
    }
}
