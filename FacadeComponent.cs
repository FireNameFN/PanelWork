using PanelWork.Entities;

namespace PanelWork;

public sealed class FacadeComponent : IComponent {
    public static int ComponentId { get; } = ComponentRegistry.Register();

    public IFacade Facade { get; set; }
}
