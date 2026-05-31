using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Components;

public sealed class ArchetypeComponent : IComponent {
    public Entity Event;

    public IEntityAction[] Constructors;

    public IEntityAction[] Destructors;

    public IEntityAction[] Events;
}
