using PanelWork.Entities;

namespace PanelWork;

public sealed class FacadeComponent : IComponent {
    public IFacade Facade { get; set; }
}
