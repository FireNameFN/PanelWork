namespace PanelWork.Entities;

public readonly struct Entity(int id, int generation) {
    public int Id { get; } = id;

    public int Generation { get; } = generation;
}
