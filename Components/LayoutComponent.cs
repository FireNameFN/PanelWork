using System.Collections.Generic;
using PanelWork.Entities;
using PanelWork.Primitives;

namespace PanelWork.Components;

public sealed class LayoutComponent : IComponent {
    public List<Entity> Children { get; set; } = [];

    public LayoutType Layout { get; set; }

    public int MinWidth { get; set; }

    public int MinHeight { get; set; }

    public int MaxWidth { get; set; } = 10000;

    public int MaxHeight { get; set; } = 10000;

    public Length Width { get; set; }

    public Length Height { get; set; }

    public Side Padding { get; set; }

    public int Gap { get; set; }

    public Box LayoutBox;
}
