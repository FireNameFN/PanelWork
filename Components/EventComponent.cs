using System.Collections.Generic;
using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Components;

public sealed class EventComponent<T> : IComponent {
    public List<IEventHandler<T>> Handlers = [];
}
