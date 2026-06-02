using System.Numerics;
using PanelWork.Primitives;
using Thermal.Core;
using Thermal.Meshes;
using Thermal.Primitives;
using Thermal.Shaders;

namespace PanelWork;

public sealed class Graphics {
    internal DrawContext drawContext;

    internal DrawHandle<Vertex, Matrix> drawHandle;

    internal Pipeline pipeline;

    Pipeline currentPipeline;

    public void UsePipeline(Pipeline pipeline) {
        if(currentPipeline == pipeline)
            return;

        drawHandle.Flush();

        drawContext.BindPipeline(pipeline);

        currentPipeline = pipeline;
    }

    public void Clear() {
        currentPipeline = null;
    }

    public void DrawRect(Box box, Vector4 color) {
        UsePipeline(pipeline);

        drawHandle.AddDraw(Rect.Create(box.X, box.Y, box.X2, box.Y2, color));
    }
}
