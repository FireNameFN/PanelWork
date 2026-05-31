namespace PanelWork.Panels;

public interface IEventHandler<T> {
    public void Handle(Panel panel, ref T e);
}
