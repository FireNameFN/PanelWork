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

        public Panel Children(params ReadOnlySpan<Panel> children) {
            LayoutComponent layout = panel.Ensure();

            if(layout.Children is null || layout.Children.Length < children.Length)
                layout.Children = new Entity[children.Length];

            for(int i = 0; i < children.Length; i++)
                layout.Children[i] = children[i].Entity;

            layout.ChildrenCount = children.Length;

            return panel;
        }

        public Panel AddChild(Panel child) {
            LayoutComponent layout = panel.Ensure();

            layout.Children ??= new Entity[4];

            if(layout.Children.Length <= layout.ChildrenCount) {
                Entity[] children = layout.Children;

                Array.Resize(ref children, children.Length * 2);

                layout.Children = children;
            }

            layout.Children[layout.ChildrenCount++] = child.Entity;

            return panel;
        }

        public Panel RemoveChild(int index) {
            LayoutComponent layout = panel.Ensure();

            int nextIndex = index + 1;

            layout.Children.AsSpan(nextIndex, layout.ChildrenCount - nextIndex).CopyTo(layout.Children.AsSpan(index));

            layout.ChildrenCount--;

            return panel;
        }

        LayoutComponent Ensure() {
            return panel.Ensure<LayoutComponent>();
        }
    }
}
