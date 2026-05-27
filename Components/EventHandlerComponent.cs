using System.Collections.Generic;
using PanelWork.Entities;

namespace PanelWork.Components;

public sealed class EventHandlerComponent<T> : IComponent {
    public List<Event1Handler> Handlers = [];
}

public delegate void Event1Handler(Entity entity, ref Event e);
