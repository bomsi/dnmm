using System;
using System.Windows.Forms;
using Triangle.Vulkan;

namespace Triangle
{
	internal class Engine : IDisposable
	{
		private readonly Window _window;

		internal Engine()
		{
			_window = new Window(Destroy, Paint);
			var applicationInfo = new VkApplicationInfo(
				VkStructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO,
				"Hello Triangle",
				VulkanHelper.VK_MAKE_VERSION(1, 0, 0),
				"No Engine",
				VulkanHelper.VK_MAKE_VERSION(1, 0, 0),
				VulkanHelper.VK_API_VERSION_1_0);
			var createInfo = new VkInstanceCreateInfo(
				VkStructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO,
				0,
				applicationInfo,
				null,
				new []{ "VK_KHR_surface" }); // TODO glfwGetRequiredInstanceExtensions
			var vkInstance = new NativeByteBuffer(32); // TODO work in progress; vkInstance type?
			unsafe
			{
				var result = NativeMethods.vkCreateInstance(&createInfo, IntPtr.Zero, vkInstance.BasePointer);
			}
			
		}

		internal void Run()
		{
			Application.Run(_window);
		}

		private void Paint()
		{
			
		}

		private void Destroy()
		{
			
		}

		private void ReleaseUnmanagedResources()
		{
			// TODO release unmanaged resources here
		}

		private void Dispose(bool disposing)
		{
			ReleaseUnmanagedResources();
			if (disposing)
			{
				_window.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Engine()
		{
			Dispose(false);
		}
	}
}
