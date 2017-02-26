namespace Triangle.Vulkan
{
	internal static class VulkanHelper
	{
		internal static uint VK_API_VERSION_1_0 = VK_MAKE_VERSION(1, 0, 0);

		internal static uint VK_MAKE_VERSION(uint major, uint minor, uint patch) => (major << 22) | (minor << 12) | patch;
	}
}
