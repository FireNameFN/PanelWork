using System;

namespace PanelWork;

public sealed class DataList<T> {
    int[] data;

    int[] reverse;

    T[] array;

    int count;

    public DataList(int capacity = 4) {
        data = new int[capacity];
        reverse = new int[capacity];
        array = new T[capacity];

        for(int i = 0; i < capacity; i++) {
            data[i] = i;
            reverse[i] = i;
        }
    }

    public int Add() {
        int index = AddIndex();

        return reverse[index];
    }

    public int Add(out T value) {
        int index = AddIndex();

        value = array[index];

        return reverse[index];
    }

    public T Get(int index) {
        return array[data[index]];
    }

    public ref T GetRef(int index) {
        return ref array[data[index]];
    }

    public void Set(int index, T value) {
        array[data[index]] = value;
    }

    public void Remove(int index) {
        int dataIndex = data[index];

        int swapIndex = --count;

        (array[dataIndex], array[swapIndex]) = (array[swapIndex], array[dataIndex]);

        (reverse[dataIndex], reverse[swapIndex]) = (reverse[swapIndex], reverse[dataIndex]);
    }

    int AddIndex() {
        int index = count++;

        if(index < data.Length)
            return index;

        int size = index * 2;

        Array.Resize(ref data, size);
        Array.Resize(ref reverse, size);
        Array.Resize(ref array, size);

        for(int i = index; i < size; i++) {
            data[i] = i;
            reverse[i] = i;
        }

        return index;
    }
}
