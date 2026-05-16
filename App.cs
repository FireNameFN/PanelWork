using System;
using System.Collections.Generic;
using PanelWork.Entities;
using SDL3;
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

    internal List<AppWindow> windows = [];

    public EntityManager entityManager = new();

    public App() {
        //SDL.SetHint("SDL_VIDEO_DRIVER", "x11");

        SDL.SetHint("SDL_VIDEO_DRIVER", "wayland,x11,cocoa,windows");

        SDL.Init(SDL.InitFlags.Video);

        SDL.VulkanLoadLibrary(null);

        string[] extensions = SDL.VulkanGetInstanceExtensions(out _);

        ThInstance instance = ThInstance.Create(VkVersion.Version_1_2, ["VK_LAYER_KHRONOS_validation"], extensions);

        ThDeviceFeatures features = new() {
            Features = new() {
                sampleRateShading = true
            }
        };

        instance.TryCreateDevicePreferDiscrete(IsPresentationSupported, ["VK_KHR_swapchain"], features, out physicalDevice, out device, out queue);

        command = new(device, queue);

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
    }

    public void Run() {
        while(true) {
            SDL.WaitEvent(out SDL.Event e);

            do {
                SDL.EventType type = (SDL.EventType)e.Type;

                if(type == SDL.EventType.WindowPixelSizeChanged) {
                    foreach(AppWindow window in windows)
                        window.Resize();
                }

                if(type == SDL.EventType.WindowCloseRequested)
                    return;
            } while(SDL.PollEvent(out e));

            foreach(AppWindow window in windows)
                window.Update();
        }
    }

    public void Dispose() {
        SDL.Quit();
    }

    static bool IsPresentationSupported(ThPhysicalDevice physicalDevice, uint queueFamily, VkQueueFlags flags) {
        return SDL.VulkanGetPresentationSupport(physicalDevice.Instance.Instance, physicalDevice.Handle, queueFamily);
    }
}
