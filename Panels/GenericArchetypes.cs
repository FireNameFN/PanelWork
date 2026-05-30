using PanelWork.Components;
using PanelWork.Facades;

namespace PanelWork.Panels;

public sealed class GenericArchetypes(PanelManager panelManager) {
    public readonly ArchetypeComponent Empty = panelManager.CreateArchetypeBuilder()
        .AddComponent<LayoutComponent>()
        .Build();

    public readonly ArchetypeComponent Rect = panelManager.CreateArchetypeBuilder()
        .AddComponent<LayoutComponent>()
        .AddComponent<RectFacadeComponent>()
        .AddEvent<RectFacadeHandler, DrawEvent>()
        .Build();
}
