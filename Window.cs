using System;
using System.Runtime.CompilerServices;
using PanelWork.Components;
using PanelWork.Entities;
using PanelWork.Facades;
using PanelWork.Layouting;
using SDL;
using Thermal.Core;
using Thermal.Extensions;
using Thermal.Primitives;
using Thermal.Shaders;
using Thermal.ThVk;
using Vortice.Vulkan;

namespace PanelWork;

public sealed unsafe class Window {
    readonly SDL_Window* handle;

    readonly App app;

    readonly Presenter presenter;

    readonly DescriptorStorage descriptorStorage;

    readonly ThCommandPool commandPool;

    readonly ThCommandBuffer commandBuffer;

    readonly VertexBuffer<Vertex> vertexBuffer;

    readonly VertexBuffer<Matrix> instanceBuffer;

    readonly DrawContext drawContext;

    readonly DrawHandle<Vertex, Matrix> drawHandle;

    readonly ThFence fence;

    ThDeviceImage colorImage;

    ThImageView colorImageView;

    ThImageView[] imageViews;

    ThFramebuffer[] framebuffers;

    readonly Graphics graphics;

    readonly ComponentLookup<LayoutComponent> layoutLookup;

    readonly ComponentLookup<FacadeComponent> facadeLookup;

    readonly ComponentLookup<ArchetypeComponent> archLookup;

    public Entity Panel { get; set; }

    internal Window(App app) {
        this.app = app;

        handle = SDL_CreateWindow("PanelWork", 1280, 720, SDL_WindowFlags.SDL_WINDOW_RESIZABLE | SDL_WindowFlags.SDL_WINDOW_VULKAN);

        VkSurfaceKHR_T* surface;

        SDL_Vulkan_CreateSurface(handle, (VkInstance_T*)app.physicalDevice.Instance.Instance.Handle, null, &surface);

        presenter = new(app.physicalDevice, app.queue, (ulong)surface) {
            Usage = VkImageUsageFlags.ColorAttachment,
            PresentMode = VkPresentModeKHR.Mailbox
        };

        descriptorStorage = new(app.device);

        commandPool = app.device.CreateCommmandPool(app.queue.QueueFamily, VkCommandPoolCreateFlags.Transient);

        commandBuffer = commandPool.AllocateCommandBuffer(VkCommandBufferLevel.Primary);

        vertexBuffer = new(app.physicalDevice, app.device);

        instanceBuffer = new(app.physicalDevice, app.device);

        drawContext = new(app.device, descriptorStorage.CreateContext(), commandBuffer.Handle);

        drawHandle = new(vertexBuffer, instanceBuffer, commandBuffer.Handle);

        fence = app.device.CreateFence();

        ShaderBuilder builder = new(app.device);

        VertexShaderLayout vertexShader = builder.BuildVertex();

        ShaderLayout solidShader = builder.BuildSolid();

        PipelineLayout pipelineLayout = PipelineLayout.Create(app.device, vertexShader, solidShader);

        Pipeline pipeline = pipelineLayout.CreatePipeline(app.renderPass.Handle, VkSampleCountFlags.Count8, -1);

        Resize();

        graphics = new() {
            drawContext = drawContext,
            drawHandle = drawHandle,
            pipeline = pipeline
        };

        layoutLookup = app.EntityManager.GetLookup<LayoutComponent>();

        facadeLookup = app.EntityManager.GetLookup<FacadeComponent>();

        archLookup = app.EntityManager.GetLookup<ArchetypeComponent>();
    }

    public void Resize() {
        if(framebuffers is not null) {
            colorImageView.Dispose();

            colorImage.Dispose();

            foreach(ThFramebuffer framebuffer in framebuffers)
                framebuffer.Dispose();

            foreach(ThImageView imageView in imageViews)
                imageView.Dispose();

            app.queue.WaitIdle();
        }

        int width;
        int height;

        SDL_GetWindowSizeInPixels(handle, &width, &height);

        presenter.SetSize(width, height);

        width = presenter.Width;
        height = presenter.Height;

        colorImage = app.device.AllocateImage(app.physicalDevice, VkFormat.B8G8R8A8Srgb, new(width, height), 1, VkSampleCountFlags.Count8, VkImageUsageFlags.ColorAttachment | VkImageUsageFlags.TransientAttachment);

        colorImageView = colorImage.Image.CreateImageView(VkFormat.B8G8R8A8Srgb, VkComponentMapping.Rgba);

        imageViews = new ThImageView[presenter.Images.Length];

        for(int i = 0; i < imageViews.Length; i++)
            imageViews[i] = presenter.Images[i].CreateImageView(VkFormat.B8G8R8A8Srgb, VkComponentMapping.Rgba);

        framebuffers = new ThFramebuffer[presenter.Images.Length];

        for(int i = 0; i < framebuffers.Length; i++)
            framebuffers[i] = app.renderPass.CreateFramebuffer([colorImageView.Handle, imageViews[i].Handle], width, height);
    }

    public void Update() {
        VkResult result = presenter.Acquire(ulong.MaxValue, out uint index);

        if(result == VkResult.ErrorOutOfDateKHR) {
            Resize();

            return;
        }

        app.layoutEngine.Update(Panel, presenter.Width, presenter.Height);

        UpdateDraw(index);
    }

    void UpdateDraw(uint index) {
        app.device.Handle.vkBeginCommandBuffer(commandBuffer.Handle, VkCommandBufferUsageFlags.OneTimeSubmit);

        commandBuffer.BeginRenderPass(app.renderPass.Handle, framebuffers[index].Handle, new(0, 0, (uint)presenter.Width, (uint)presenter.Height), new(0, 1, 0, 1), VkSubpassContents.Inline);

        app.device.Handle.vkCmdSetViewport(commandBuffer.Handle, 0, new VkViewport(presenter.Width, presenter.Height));

        app.device.Handle.vkCmdSetScissor(commandBuffer.Handle, 0, new VkRect2D(0, 0, (uint)presenter.Width, (uint)presenter.Height));

        //

        drawHandle.WithInstance([Matrix.CreateFrom(Matrix.CreateViewport(presenter.Width, presenter.Height))]);

        UpdateDrawEntity(Panel);

        drawHandle.Flush();

        graphics.Clear();

        //

        app.device.Handle.vkCmdEndRenderPass(commandBuffer.Handle);

        app.device.Handle.vkEndCommandBuffer(commandBuffer.Handle);

        drawHandle.BufferFlush();

        app.queue.Submit(fence.Handle, [presenter.Semaphore.Handle], [VkPipelineStageFlags.ColorAttachmentOutput], [commandBuffer.Handle], [presenter.PresentSemaphores[index].Handle]);

        presenter.Present(index);

        fence.Wait();

        fence.Reset();

        commandPool.Reset();

        descriptorStorage.Clear();

        drawHandle.BufferClear();
    }

    void UpdateDrawEntity(Entity entity) {
        LayoutComponent layout = layoutLookup.Get(entity);

        DrawEvent e = new() {
            Graphics = graphics,
            Box = layout.LayoutBox
        };

        if(archLookup.TryGet(entity, out ArchetypeComponent arch))
            foreach(Event1Handler action in app.EntityManager.GetComponent<EventHandlerComponent<DrawEvent>>(arch.Event).Handlers)
                action(entity, ref Unsafe.As<DrawEvent, Event>(ref e));

        for(int i = 0; i < layout.PanelCount; i++)
            UpdateDrawEntity(layout.Panels[i]);
    }
}
