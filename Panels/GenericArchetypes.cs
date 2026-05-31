using PanelWork.Components;
using PanelWork.Facades;

namespace PanelWork.Panels;

public sealed class GenericArchetypes(PanelManager panelManager) {
    public readonly ArchetypeComponent Empty = ArchetypeBuilder.Create()
        .AddComponent<LayoutComponent>()
        .Build(panelManager);

    public readonly ArchetypeComponent Rect = ArchetypeBuilder.Create()
        .AddComponent<LayoutComponent>()
        .AddComponent<RectFacadeComponent>()
        .AddEvent<RectFacadeHandler, DrawEvent>()
        .Build(panelManager);
}
