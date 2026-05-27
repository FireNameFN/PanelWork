using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using PanelWork.Components;
using PanelWork.Entities;
using PanelWork.Facades;
using PanelWork.Layouting;
using SDL;
using Thermal.Core;
using Thermal.Extensions;
using Thermal.ThVk;
using Vortice.Vulkan;

namespace PanelWork;

public sealed class App : IDisposable {
    internal readonly ThPhysicalDevice physicalDevice;

    internal readonly ThDevice device;

    internal readonly ThQueue queue;

    internal readonly ThRenderPass renderPass;

    internal readonly Command command;

    internal readonly LayoutEngine layoutEngine;

    readonly List<Window> windows = [];

    public EntityManager EntityManager { get; } = new();

    public Entity arch;

    public ArchetypeComponent archComp;

    public unsafe App() {
        //SDL.SetHint("SDL_VIDEO_DRIVER", "x11");

        SDL_SetHint("SDL_VIDEO_DRIVER", "wayland,x11,cocoa,windows");

        SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO);

        SDL_Vulkan_LoadLibrary((byte*)null);

        uint extensionsCount;

        byte** extensionsPointer = SDL_Vulkan_GetInstanceExtensions(&extensionsCount);

        string[] extensions = new string[extensionsCount];

        for(int i = 0; i < extensionsCount; i++)
            extensions[i] = Utf8StringMarshaller.ConvertToManaged(extensionsPointer[i]);

        ThInstance instance = ThInstance.Create(VkVersion.Version_1_2, ["VK_LAYER_KHRONOS_validation"], extensions);

        ThDeviceFeatures features = new() {
            Features = new() {
                sampleRateShading = true
            }
        };

        instance.TryCreateDevicePreferDiscrete(IsPresentationSupported, ["VK_KHR_swapchain"], features, out physicalDevice, out device, out queue);

        command = new(queue);

        VkAttachmentDescription colorAttachment = new() {
            format = VkFormat.B8G8R8A8Srgb,
            samples = VkSampleCountFlags.Count8,
            loadOp = VkAttachmentLoadOp.Clear,
            storeOp = VkAttachmentStoreOp.DontCare,
            stencilLoadOp = VkAttachmentLoadOp.DontCare,
            stencilStoreOp = VkAttachmentStoreOp.DontCare,
            initialLayout = VkImageLayout.Undefined,
            finalLayout = VkImageLayout.ColorAttachmentOptimal
        };

        VkAttachmentDescription resolveAttachment = new() {
            format = VkFormat.B8G8R8A8Srgb,
            samples = VkSampleCountFlags.Count1,
            loadOp = VkAttachmentLoadOp.DontCare,
            storeOp = VkAttachmentStoreOp.Store,
            stencilLoadOp = VkAttachmentLoadOp.DontCare,
            stencilStoreOp = VkAttachmentStoreOp.DontCare,
            initialLayout = VkImageLayout.Undefined,
            finalLayout = VkImageLayout.PresentSrcKHR
        };

        ThRenderPass.SubpassDescriptionSpan subpassDescriptionSpan = new() {
            PipelineBindPoints = [VkPipelineBindPoint.Graphics],
            Input = new([0], []),
            Color = new([1], [new VkAttachmentReference(0, VkImageLayout.ColorAttachmentOptimal)]),
            Resolve = new([1], [new VkAttachmentReference(1, VkImageLayout.ColorAttachmentOptimal)]),
            Depth = new([0], []),
            Preserve = new([0], [])
        };

        renderPass = device.CreateRenderPass([colorAttachment, resolveAttachment], subpassDescriptionSpan);

        layoutEngine = new(this);

        arch = EntityManager.CreateEntity();

        archComp = EntityManager.EnsureComponent<ArchetypeComponent>(arch);

        archComp.Event = EntityManager.CreateEntity();

        EventHandlerComponent<DrawEvent> drawHandlers = EntityManager.EnsureComponent<EventHandlerComponent<DrawEvent>>(archComp.Event);

        drawHandlers.Handlers.Add((entity, ref e) => {
            DrawEvent drawEvent = Unsafe.As<Event, DrawEvent>(ref e);

            if(EntityManager.TryGetComponent(entity, out FacadeComponent facade))
                facade.Facade.Draw(drawEvent.Graphics, drawEvent.Box);
        });
    }

    public Window CreateWindow() {
        Window window = new(this);

        windows.Add(window);

        return window;
    }

    public Panel CreatePanel() {
        return new(EntityManager, EntityManager.CreateEntity());
    }

    public unsafe void Run() {
        while(true) {
            SDL_Event e;

            SDL_PollEvent(&e);

            do {
                SDL_EventType type = e.Type;

                if(type == SDL_EventType.SDL_EVENT_WINDOW_PIXEL_SIZE_CHANGED) {
                    foreach(Window window in windows)
                        window.Resize();
                }

                if(type == SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED)
                    return;
            } while(SDL_PollEvent(&e));

            foreach(Window window in windows)
                window.Update();
        }
    }

    public void Dispose() {
        SDL_Quit();
    }

    static unsafe bool IsPresentationSupported(ThPhysicalDevice physicalDevice, uint queueFamily, VkQueueFlags flags) {
        return SDL_Vulkan_GetPresentationSupport((VkInstance_T*)physicalDevice.Instance.Instance.Handle, (VkPhysicalDevice_T*)physicalDevice.Handle.Handle, queueFamily);
    }
}
