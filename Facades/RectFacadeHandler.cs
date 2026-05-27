using PanelWork.Entities;
using PanelWork.Panels;

namespace PanelWork.Facades;

public sealed class RectFacadeHandler : IEventHandler<DrawEvent> {
    ComponentLookup<RectFacadeComponent> facadeLookup;

    public void Initialize(PanelManager panelManager) {
        facadeLookup = panelManager.EntityManager.GetLookup<RectFacadeComponent>();
    }

    public void Handle(Entity entity, ref DrawEvent e) {
        RectFacadeComponent facade = facadeLookup.Get(entity);

        e.Graphics.DrawRect(e.Box.X, e.Box.Y, e.Box.Width, e.Box.Height, facade.Color);
    }
}
