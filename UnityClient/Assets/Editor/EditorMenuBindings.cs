using System.Diagnostics;
using System.IO;
using UnityEditor;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	internal static class EditorMenuBindings
	{
		[MenuItem("Development/Asset Bundles/Build for Android")]
		internal static void BuildAllAssetBundlesForAndroid()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Android");

			AssetBundleBuilder assetBundleBuilder = new AssetBundleBuilder();
			assetBundleBuilder.BuildAllAssetBundles("Android", assetBundleDirectory);
		}

		[MenuItem("Development/Asset Bundles/Build for Linux")]
		internal static void BuildAllAssetBundlesForLinux()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Linux");

			AssetBundleBuilder assetBundleBuilder = new AssetBundleBuilder();
			assetBundleBuilder.BuildAllAssetBundles("Linux", assetBundleDirectory);
		}

		[MenuItem("Development/Asset Bundles/Build for Windows")]
		internal static void BuildAllAssetBundlesForWindows()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Windows");

			AssetBundleBuilder assetBundleBuilder = new AssetBundleBuilder();
			assetBundleBuilder.BuildAllAssetBundles("Windows", assetBundleDirectory);
		}

		[MenuItem("Development/Applications/Build for Android (Debug)")]
		internal static void BuildApplicationForAndroidDebug()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Android");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Android-Debug");

			ApplicationBuilder applicationBuilder = new ApplicationBuilder();
			applicationBuilder.BuildApplication("Android", "Debug", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Android (Release)")]
		internal static void BuildApplicationForAndroidRelease()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Android");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Android-Release");

			ApplicationBuilder applicationBuilder = new ApplicationBuilder();
			applicationBuilder.BuildApplication("Android", "Release", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Linux (Debug)")]
		internal static void BuildApplicationForLinuxDebug()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Linux");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Linux-Debug");

			ApplicationBuilder applicationBuilder = new ApplicationBuilder();
			applicationBuilder.BuildApplication("Linux", "Debug", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Linux (Release)")]
		internal static void BuildApplicationForLinuxRelease()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Linux");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Linux-Release");

			ApplicationBuilder applicationBuilder = new ApplicationBuilder();
			applicationBuilder.BuildApplication("Linux", "Release", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Windows (Debug)")]
		internal static void BuildApplicationForWindowsDebug()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Windows");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Windows-Debug");

			ApplicationBuilder applicationBuilder = new ApplicationBuilder();
			applicationBuilder.BuildApplication("Windows", "Debug", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Windows (Release)")]
		internal static void BuildApplicationForWindowsRelease()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Windows");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Windows-Release");

			ApplicationBuilder applicationBuilder = new ApplicationBuilder();
			applicationBuilder.BuildApplication("Windows", "Release", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}
	}
}
