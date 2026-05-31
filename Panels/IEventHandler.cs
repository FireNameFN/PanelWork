using PanelWork.Entities;

namespace PanelWork.Panels;

public interface IEventHandler {
    public void Initialize(PanelManager panelManager);
}

public interface IEventHandler<T> : IEventHandler {
    //public void Initialize(PanelManager panelManager);

    public void Handle(Entity entity, ref T e);
}
