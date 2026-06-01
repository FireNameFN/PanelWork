using System;
using System.Threading.Tasks;
using PanelWork.Entities;

namespace PanelWork.Interactions;

public sealed class ButtonComponent : IComponent {
    public Func<Task> Action;
}
