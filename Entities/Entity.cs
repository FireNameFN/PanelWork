namespace PanelWork.Entities;

public readonly struct Entity(int id, int generation) {
    public readonly int Id = id;

    public readonly int Generation = generation;
}
