using PanelWork.Components;
using PanelWork.Facades;
using PanelWork.Interactions;

namespace PanelWork.Panels;

public sealed class GenericArchetypes {
    public readonly ArchetypeComponent Empty;

    public readonly ArchetypeComponent Rect;

    public readonly ArchetypeComponent Focus;

    public GenericArchetypes(PanelManager panelManager) {
        ArchetypeBuilder builder = ArchetypeBuilder.Create();

        Empty = builder
            .AddComponent<LayoutComponent>()
            .Build(panelManager);

        Rect = builder
            .AddComponent<RectFacadeComponent>()
            .AddEvent<RectFacadeHandler, DrawEvent>()
            .Build(panelManager);

        Focus = builder
            .AddComponent<FocusComponent>()
            .AddEvent<FocusHandler, InteractionEvent>()
            .Build(panelManager);
    }
}
