using System;
using System.Threading.Tasks;
using PanelWork.Entities;

namespace PanelWork.Interactions;

public sealed class ActionComponent<T> : IComponent {
    public Func<T, Task> Action;
}
