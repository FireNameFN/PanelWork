using PanelWork.Panels;

namespace PanelWork.Interactions;

public static class ActionArchetypeExtensions {
    extension(ArchetypeBuilder builder) {
        public ArchetypeBuilder AddAction<T>() {
            return builder
                .AddComponent<ActionComponent<T>>()
                .AddEvent<ActionHandler<T>, T>();
        }
    }
}
