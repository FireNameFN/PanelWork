using System.Collections.Generic;
using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Components;

public sealed class EventComponent<T> : IComponent, IEventComponent {
    public List<IEventHandler<T>> Handlers = [];

    public void Add(IEventHandler handler) {
        Handlers.Add((IEventHandler<T>)handler);
    }
}
