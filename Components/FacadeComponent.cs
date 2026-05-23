using PanelWork.Entities;
using PanelWork.Facades;

namespace PanelWork.Components;

public sealed class FacadeComponent : IComponent {
    public IFacade Facade { get; set; }
}
