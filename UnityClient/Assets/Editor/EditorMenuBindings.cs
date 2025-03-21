using BenjaminHamon.Solitaire.UnityClient.Runtime;
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

			ApplicationInformation applicationInformation = new ApplicationInformationImplementation();
			ApplicationBuilder applicationBuilder = new ApplicationBuilder(applicationInformation, UnityApplicationFactory.CreateSerializer());
			applicationBuilder.BuildApplication("Android", "Debug", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Android (Release)")]
		internal static void BuildApplicationForAndroidRelease()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Android");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Android-Release");

			ApplicationInformation applicationInformation = new ApplicationInformationImplementation();
			ApplicationBuilder applicationBuilder = new ApplicationBuilder(applicationInformation, UnityApplicationFactory.CreateSerializer());
			applicationBuilder.BuildApplication("Android", "Release", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Linux (Debug)")]
		internal static void BuildApplicationForLinuxDebug()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Linux");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Linux-Debug");

			ApplicationInformation applicationInformation = new ApplicationInformationImplementation();
			ApplicationBuilder applicationBuilder = new ApplicationBuilder(applicationInformation, UnityApplicationFactory.CreateSerializer());
			applicationBuilder.BuildApplication("Linux", "Debug", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Linux (Release)")]
		internal static void BuildApplicationForLinuxRelease()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Linux");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Linux-Release");

			ApplicationInformation applicationInformation = new ApplicationInformationImplementation();
			ApplicationBuilder applicationBuilder = new ApplicationBuilder(applicationInformation, UnityApplicationFactory.CreateSerializer());
			applicationBuilder.BuildApplication("Linux", "Release", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Windows (Debug)")]
		internal static void BuildApplicationForWindowsDebug()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Windows");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Windows-Debug");

			ApplicationInformation applicationInformation = new ApplicationInformationImplementation();
			ApplicationBuilder applicationBuilder = new ApplicationBuilder(applicationInformation, UnityApplicationFactory.CreateSerializer());
			applicationBuilder.BuildApplication("Windows", "Debug", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}

		[MenuItem("Development/Applications/Build for Windows (Release)")]
		internal static void BuildApplicationForWindowsRelease()
		{
			string assetBundleDirectory = Path.Combine("..", "Artifacts", "AssetBundles", "Windows");
			string outputDirectory = Path.Combine("..", "Artifacts", "Applications", "Windows-Release");

			ApplicationInformation applicationInformation = new ApplicationInformationImplementation();
			ApplicationBuilder applicationBuilder = new ApplicationBuilder(applicationInformation, UnityApplicationFactory.CreateSerializer());
			applicationBuilder.BuildApplication("Windows", "Release", assetBundleDirectory, outputDirectory);
			Process.Start(outputDirectory);
		}
	}
}
