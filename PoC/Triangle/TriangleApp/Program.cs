using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

using TriangleLib;

namespace TriangleApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var processHandle = Process.GetCurrentProcess().SafeHandle;
			var window = GetPlatformWindow();

			// TODO why does (windowInstance as IWindow) == null ?
			Console.WriteLine(window == null ? "Could not cast window instance to IWindow.": "Window instance casted to IWindow.");

			Console.ReadLine();
		}

		private static IWindow GetPlatformWindow()
		{
			// TODO currently only Win32 supported
			const string assemblyName = "TriangleLib.Win32.dll";
			
			var assemblyPath = $"{Path.GetFullPath(".")}{Path.DirectorySeparatorChar}";

			if (!File.Exists(assemblyPath))
			{
				// TODO dirty but better than hardcoding working directory?
				assemblyPath = $"{Path.GetFullPath(".")}{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";
#if DEBUG
				assemblyPath += $"Debug{Path.DirectorySeparatorChar}";
#elif RELEASE
				assemblyPath += $"Release{Path.DirectorySeparatorChar}";
#endif
#if NETCOREAPP1_1
				assemblyPath += $"netcoreapp1.1{Path.DirectorySeparatorChar}";
#endif
			}
			var assemblyLoader = new AssemblyLoader(assemblyPath);
			var assembly = assemblyLoader.LoadFromAssemblyPath(assemblyPath + assemblyName);

			var windowType = assembly.GetType("TriangleLib.Win32.Window");
			var windowInstance = Activator.CreateInstance(windowType);

			Console.WriteLine(windowInstance == null? "Window instance not created." : "Window instance created.");

			return windowInstance as IWindow;
		}

		private class AssemblyLoader : AssemblyLoadContext
		{
			// class adapted from http://stackoverflow.com/a/38843565

			private readonly string _folderPath;

			public AssemblyLoader(string folderPath)
			{
				_folderPath = folderPath;
			}

			protected override Assembly Load(AssemblyName assemblyName)
			{
				var deps = DependencyContext.Default;
				var res = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name)).ToList();
				if (res.Count > 0)
				{
					return Assembly.Load(new AssemblyName(res.First().Name));
				}
				var apiApplicationFileInfo = new FileInfo($"{_folderPath}{Path.DirectorySeparatorChar}{assemblyName.Name}.dll");
				if (File.Exists(apiApplicationFileInfo.FullName))
				{
					var asl = new AssemblyLoader(apiApplicationFileInfo.DirectoryName);
					return asl.LoadFromAssemblyPath(apiApplicationFileInfo.FullName);
				}
				return Assembly.Load(assemblyName);
			}

		}
	}
}