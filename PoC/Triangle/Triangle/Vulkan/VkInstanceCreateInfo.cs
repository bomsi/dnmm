using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Triangle.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct VkInstanceCreateInfo
	{
		internal VkStructureType sType;
		internal void* pNext;
		internal uint flags;
		internal VkApplicationInfo pApplicationInfo;
		internal uint enabledLayerCount;
		internal byte** ppEnabledLayerNames;
		internal uint enabledExtensionCount;
		internal byte** ppEnabledExtensionNames;

		internal VkInstanceCreateInfo(VkStructureType sType, uint flags, VkApplicationInfo applicationInfo, string[] enabledLayerNames, string[] enabledExtensionNames)
		{
			this.sType = sType;
			pNext = null;
			this.flags = flags;
			pApplicationInfo = applicationInfo;
			if (enabledLayerNames != null)
			{
				enabledLayerCount = (uint)enabledLayerNames.Length;
				ppEnabledLayerNames = ToArray(enabledLayerNames);
			}
			else
			{
				enabledLayerCount = 0;
				ppEnabledLayerNames = null;
			}
			if (enabledExtensionNames != null)
			{
				enabledExtensionCount = (uint)enabledExtensionNames.Length;
				ppEnabledExtensionNames = ToArray(enabledExtensionNames);
			}
			else
			{
				enabledExtensionCount = 0;
				ppEnabledExtensionNames = null;
			}
		}

		private static byte** ToArray(string[] array)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			var arrayBuffer = new NativeByteArrayBuffer(array.Length);
			for (var element = 0; element < array.Length; ++element)
			{
				var bytes = Encoding.ASCII.GetBytes(array[element]);
				var stringBuffer = new NativeByteBuffer(bytes.Length);
				for (var i = 0; i < bytes.Length; ++i)
					stringBuffer[i] = bytes[i];
				arrayBuffer[element] = stringBuffer.BasePointer;
			}
			return arrayBuffer.BasePointer;
		}
	}
}
