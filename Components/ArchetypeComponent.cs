using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Components;

public sealed class ArchetypeComponent : IComponent {
    public Entity Event;

    public int[] Components;

    public (int Event, IEventHandler Handler)[] Events;
}
