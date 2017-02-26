using System;
using System.Runtime.InteropServices;
using Triangle.Vulkan;

namespace Triangle
{
	internal static class NativeMethods
	{
		public const int MEM_COMMIT = 0x00001000;
		public const int MEM_RESERVE = 0x00002000;
		public const int MEM_RELEASE = 0x8000;
		public const int PAGE_READWRITE = 0x04;

#if WIN64

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern unsafe void* VirtualAlloc(void* address, long size, int allocationType, int protect);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern unsafe int VirtualFree(void* address, long size, int freeType);
#else
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern unsafe void* VirtualAlloc(void* address, int size, int allocationType, int protect);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern unsafe int VirtualFree(void* address, int size, int freeType);
#endif

		[DllImport("vulkan-1.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern unsafe VkResult vkCreateInstance(VkInstanceCreateInfo* pCreateInfo, IntPtr pAllocator, byte* pInstance); // TODO arguments don't match?
	}
}
