using PanelWork.Entities;

namespace PanelWork.Components;

public sealed class FacadeComponent : IComponent {
    public IFacade Facade { get; set; }
}
