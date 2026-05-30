using PanelWork.Entities;
using PanelWork.Primitives;

namespace PanelWork.Components;

public sealed class LayoutComponent : IComponent {
    public Entity[] Panels;

    public int PanelCount;

    public LayoutType Layout;

    public int MinWidth;

    public int MinHeight;

    public int MaxWidth;

    public int MaxHeight;

    public double StarWidth;

    public double StarHeight;

    public Side Padding;

    public int Gap;

    public Box LayoutBox;
}
