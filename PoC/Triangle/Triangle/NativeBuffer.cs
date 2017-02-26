using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Triangle
{
	internal abstract class NativeBuffer : IDisposable
	{
		protected readonly unsafe void* Buffer;
		private readonly int _elementSize;
		private readonly int _elementCount;
		private readonly int _bufferSize;

		/// <summary>
		/// Initializes a buffer instance.
		/// </summary>
		/// <param name="elementSize">Element size in bytes.</param>
		/// <param name="elementCount">Number of elements allowed in the buffer.</param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="OverflowException"></exception>
		/// <exception cref="Win32Exception"></exception>
		protected NativeBuffer(int elementSize, int elementCount)
		{
			if (elementSize < 1)
				throw new ArgumentException(nameof(elementSize));
			if (elementCount < 1)
				throw new ArgumentException(nameof(elementCount));

			checked
			{
				_bufferSize = elementSize * elementCount;
			}
			unsafe
			{
				var p = NativeMethods.VirtualAlloc((void*)0, _bufferSize, NativeMethods.MEM_COMMIT | NativeMethods.MEM_RESERVE, NativeMethods.PAGE_READWRITE);
				if (p == (void*)0)
					throw new Win32Exception(Marshal.GetLastWin32Error());
				Buffer = p;
			}
			_elementSize = elementSize;
			_elementCount = elementCount;
		}

		/// <summary>
		/// Releases underlying memory.
		/// </summary>
		/// <exception cref="Win32Exception"></exception>
		private void ReleaseUnmanagedResources()
		{
			unsafe
			{
				var result = NativeMethods.VirtualFree(Buffer, 0, NativeMethods.MEM_RELEASE);
				if (result == 0)
					throw new Win32Exception(Marshal.GetLastWin32Error());
			}
		}

		/// <summary>
		/// Disposes the buffer.
		/// </summary>
		public void Dispose()
		{
			ReleaseUnmanagedResources();
			GC.SuppressFinalize(this);
		}

		~NativeBuffer()
		{
			ReleaseUnmanagedResources();
		}

		/// <summary>
		/// Validate if access to the given index is out of bounds.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <exception cref="IndexOutOfRangeException">Thrown if the index is out of range.</exception>
		/// <exception cref="OverflowException">Thrown if index offset calculation causes overflow.</exception>
		protected void ValidateIndex(int index)
		{
			if (index < 0)
				throw new IndexOutOfRangeException();
			if (index >= _elementCount)
				throw new IndexOutOfRangeException();
			checked
			{
				if (index * _elementSize >= _bufferSize)
					throw new IndexOutOfRangeException();
			}
		}
	}

	internal sealed class NativeIntBuffer : NativeBuffer
	{
		internal NativeIntBuffer(int bufferSize) :
			base(sizeof(int), bufferSize)
		{
		}

		public int this[int index]
		{
			get
			{
				ValidateIndex(index);
				unsafe
				{
					return ((int*)Buffer)[index];
				}
			}
			set
			{
				ValidateIndex(index);
				unsafe
				{
					((int*)Buffer)[index] = value;
				}
			}
		}

		public unsafe int* BasePointer => (int*)Buffer;
	}

	internal sealed class NativeUIntBuffer : NativeBuffer
	{
		internal NativeUIntBuffer(int bufferSize) :
			base(sizeof(uint), bufferSize)
		{
		}

		public uint this[int index]
		{
			get
			{
				ValidateIndex(index);
				unsafe
				{
					return ((uint*)Buffer)[index];
				}
			}
			set
			{
				ValidateIndex(index);
				unsafe
				{
					((uint*)Buffer)[index] = value;
				}
			}
		}

		public unsafe uint* BasePointer => (uint*)Buffer;
	}

	internal sealed class NativeFloatBuffer : NativeBuffer
	{
		internal NativeFloatBuffer(int bufferSize) :
			base(sizeof(int), bufferSize)
		{
		}

		public float this[int index]
		{
			get
			{
				ValidateIndex(index);
				unsafe
				{
					return ((float*)Buffer)[index];
				}
			}
			set
			{
				ValidateIndex(index);
				unsafe
				{
					((float*)Buffer)[index] = value;
				}
			}
		}

		public unsafe float* BasePointer => (float*)Buffer;
	}

	internal sealed class NativeByteBuffer : NativeBuffer
	{
		internal NativeByteBuffer(int bufferSize) :
			base(sizeof(byte), bufferSize)
		{
		}

		public byte this[int index]
		{
			get
			{
				ValidateIndex(index);
				unsafe
				{
					return ((byte*)Buffer)[index];
				}
			}
			set
			{
				ValidateIndex(index);
				unsafe
				{
					((byte*)Buffer)[index] = value;
				}
			}
		}

		public unsafe byte* BasePointer => (byte*)Buffer;
	}

	internal sealed unsafe class NativeByteArrayBuffer : NativeBuffer
	{
		internal NativeByteArrayBuffer(int bufferSize) :
			base(sizeof(byte*), bufferSize)
		{
		}

		public byte* this[int index]
		{
			get
			{
				ValidateIndex(index);
				return ((byte**)Buffer)[index];
			}
			set
			{
				ValidateIndex(index);
				((byte**)Buffer)[index] = value;
			}
		}

		public byte** BasePointer => (byte**)Buffer;
	}
}
