using PanelWork.Panels;

namespace PanelWork.Facades;

public sealed class RectFacadeHandler : IEventHandler<DrawEvent> {
    public void Handle(Panel panel, ref DrawEvent e) {
        RectFacadeComponent facade = panel.Get<RectFacadeComponent>();

        e.Graphics.DrawRect(e.Box.X, e.Box.Y, e.Box.Width, e.Box.Height, facade.Color);
    }
}
