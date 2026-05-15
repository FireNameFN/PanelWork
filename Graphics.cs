using System.Numerics;
using Thermal.Core;
using Thermal.Meshes;
using Thermal.Primitives;
using Thermal.Shaders;

namespace PanelWork;

public sealed class Graphics {
    internal DrawContext drawContext;

    internal DrawHandle<Vertex, Matrix> drawHandle;

    internal Pipeline pipeline;

    public void DrawRect(int x, int y, int width, int height, Vector4 color) {
        drawContext.BindPipeline(pipeline);

        drawHandle.WithInstance([Matrix.CreateFrom(Matrix.CreateViewport(1280, 720))]);

        drawHandle.AddDraw(Rect.Create(x, y, x + width, y + height, color));

        drawHandle.Flush();
    }
}
