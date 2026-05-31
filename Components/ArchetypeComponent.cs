using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Components;

public sealed class ArchetypeComponent : IComponent {
    public Entity Event;

    public IConstructor[] Constructors;

    public IConstructor[] Destructors;

    public (int Event, IEventHandler Handler)[] Events;
}
