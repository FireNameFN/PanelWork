using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Components;

public sealed class HandlerComponent<T> : IComponent {
    public IEventHandler<T> Handler;
}
