using PanelWork.Entities;

namespace PanelWork.Panels;

public interface IEventHandler<T> : ISingleton {
    public void Initialize(PanelManager panelManager);

    public void Handle(Entity entity, ref T e);
}
