using PanelWork.Primitives;

namespace PanelWork.Components;

public static class LayoutPanelExtensions {
    extension(Panel panel) {
        public Panel Min(int width, int height) {
            LayoutComponent layout = panel.Ensure();

            layout.MinWidth = width;
            layout.MinHeight = height;

            return panel;
        }

        public Panel Max(int width, int height) {
            LayoutComponent layout = panel.Ensure();

            layout.MaxWidth = width;
            layout.MaxHeight = height;

            return panel;
        }

        public Panel StarWidth(double star = 1) {
            LayoutComponent layout = panel.Ensure();

            layout.Width = Length.Star(star);

            return panel;
        }

        public Panel StarHeight(double star = 1) {
            LayoutComponent layout = panel.Ensure();

            layout.Height = Length.Star(star);

            return panel;
        }

        public Panel Padding(Side padding) {
            LayoutComponent layout = panel.Ensure();

            layout.Padding = padding;

            return panel;
        }

        public Panel Gap(int gap) {
            LayoutComponent layout = panel.Ensure();

            layout.Gap = gap;

            return panel;
        }

        public Panel Add(Panel child) {
            LayoutComponent layout = panel.Ensure();

            layout.Children.Add(child.Entity);

            return panel;
        }

        LayoutComponent Ensure() {
            return panel.EnsureComponent<LayoutComponent>();
        }
    }
}
