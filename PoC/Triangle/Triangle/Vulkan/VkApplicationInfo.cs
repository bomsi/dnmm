using System.Runtime.InteropServices;
using System.Text;

namespace Triangle.Vulkan
{
	[StructLayout(LayoutKind.Sequential)]
	internal unsafe struct VkApplicationInfo
	{
		internal VkStructureType sType;
		internal void* pNext;
		internal byte* pApplicationName;
		internal uint applicationVersion;
		internal byte* pEngineName;
		internal uint engineVersion;
		internal uint apiVersion;

		internal VkApplicationInfo(VkStructureType sType, string applicationName, uint applicationVersion, string engineName, uint engineVersion, uint apiVersion)
		{
			this.sType = sType;
			pNext = null;
			var bytes = Encoding.ASCII.GetBytes(applicationName);
			var applicationNameBuffer = new NativeByteBuffer(bytes.Length);
			for (var i = 0; i < bytes.Length; ++i)
				applicationNameBuffer[i] = bytes[i];
			pApplicationName = applicationNameBuffer.BasePointer;
			this.applicationVersion = applicationVersion;
			bytes = Encoding.ASCII.GetBytes(engineName);
			var engineNameBuffer = new NativeByteBuffer(bytes.Length);
			for (var i = 0; i < bytes.Length; ++i)
				engineNameBuffer[i] = bytes[i];
			pEngineName = engineNameBuffer.BasePointer;
			this.engineVersion = engineVersion;
			this.apiVersion = apiVersion;
		}
	}
}
