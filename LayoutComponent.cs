using System.Collections.Generic;

namespace PanelWork;

public sealed class LayoutComponent : IComponent {
    public static int ComponentId { get; } = ComponentRegistry.Register();

    public List<Entity> Children { get; set; } = [];

    public LayoutType Layout { get; set; }

    public int MinWidth { get; set; }

    public int MinHeight { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public Side Padding { get; set; }

    public int Gap { get; set; }
}
