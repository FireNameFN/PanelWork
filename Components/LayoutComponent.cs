using System.Collections.Generic;
using PanelWork.Entities;
using PanelWork.Primitives;

namespace PanelWork.Components;

public sealed class LayoutComponent : IComponent {
    public List<Entity> Children { get; set; } = [];

    public LayoutType Layout { get; set; }

    public int MinWidth { get; set; }

    public int MinHeight { get; set; }

    public int MaxWidth { get; set; }

    public int MaxHeight { get; set; }

    public double StarWidth { get; set; }

    public double StarHeight { get; set; }

    public Side Padding { get; set; }

    public int Gap { get; set; }

    public Box LayoutBox;
}
