using System;
using PanelWork.Entities;
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

        public Panel MinWidth(int width) {
            LayoutComponent layout = panel.Ensure();

            layout.MinWidth = width;

            return panel;
        }

        public Panel MinHeight(int height) {
            LayoutComponent layout = panel.Ensure();

            layout.MinHeight = height;

            return panel;
        }

        public Panel Max(int width, int height) {
            LayoutComponent layout = panel.Ensure();

            layout.MaxWidth = width;
            layout.MaxHeight = height;

            return panel;
        }

        public Panel MaxWidth(int width) {
            LayoutComponent layout = panel.Ensure();

            layout.MaxWidth = width;

            return panel;
        }

        public Panel MaxHeight(int height) {
            LayoutComponent layout = panel.Ensure();

            layout.MaxHeight = height;

            return panel;
        }

        public Panel Star(double width, double height) {
            LayoutComponent layout = panel.Ensure();

            layout.StarWidth = width;
            layout.StarHeight = height;

            return panel;
        }

        public Panel StarWidth(double star) {
            LayoutComponent layout = panel.Ensure();

            layout.StarWidth = star;

            return panel;
        }

        public Panel StarHeight(double star) {
            LayoutComponent layout = panel.Ensure();

            layout.StarHeight = star;

            return panel;
        }

        public Panel GrowWidth() {
            LayoutComponent layout = panel.Ensure();

            layout.MaxWidth = int.MaxValue;
            layout.StarWidth = 1;

            return panel;
        }

        public Panel GrowHeight() {
            LayoutComponent layout = panel.Ensure();

            layout.MaxHeight = int.MaxValue;
            layout.StarHeight = 1;

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

        public Panel Panels(params ReadOnlySpan<Panel> panels) {
            LayoutComponent layout = panel.Ensure();

            if(layout.Panels is null || layout.Panels.Length < panels.Length)
                layout.Panels = new Entity[panels.Length];

            for(int i = 0; i < panels.Length; i++)
                layout.Panels[i] = panels[i].Entity;

            layout.PanelCount = panels.Length;

            return panel;
        }

        public Panel AddPanel(Panel subpanel) {
            LayoutComponent layout = panel.Ensure();

            layout.Panels ??= new Entity[4];

            if(layout.Panels.Length <= layout.PanelCount) {
                Entity[] panels = layout.Panels;

                Array.Resize(ref panels, panels.Length * 2);

                layout.Panels = panels;
            }

            layout.Panels[layout.PanelCount++] = subpanel.Entity;

            return panel;
        }

        public Panel RemovePanel(int index) {
            LayoutComponent layout = panel.Ensure();

            int nextIndex = index + 1;

            layout.Panels.AsSpan(nextIndex, layout.PanelCount - nextIndex).CopyTo(layout.Panels.AsSpan(index));

            layout.PanelCount--;

            return panel;
        }

        LayoutComponent Ensure() {
            return panel.Ensure<LayoutComponent>();
        }
    }
}
