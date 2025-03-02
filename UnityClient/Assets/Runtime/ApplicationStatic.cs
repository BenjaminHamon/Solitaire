using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using System;
using System.IO;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>Global static class to keep state between scenes.</summary>
	public static class ApplicationStatic
    {
		static ApplicationStatic()
		{
			Application = new Model.Application();
			AssetLoader = CreateAssetLoader();
		}

		public static readonly Model.Application Application;

#if UNITY_EDITOR
		private static bool UseAssetBundlesInEditor = false;
		private static string AssetBundlesPathInEditor = Path.Combine("..", "Artifacts", "AssetBundles", GetAssetBundlePlatform(UnityEngine.Application.platform));
#endif

		public static readonly AssetLoader<UnityEngine.Object> AssetLoader;

		private static AssetLoader<UnityEngine.Object> CreateAssetLoader()
		{
#if UNITY_EDITOR
			if (UnityEngine.Application.isEditor)
			{
				if (UseAssetBundlesInEditor)
				{
					return new AssetLoaderUsingBundles(AssetBundlesPathInEditor);
				}

				return new EditorAssetLoader();
			}
#endif

			return new AssetLoaderUsingBundles(Path.Combine(UnityEngine.Application.streamingAssetsPath, "AssetBundles"));
		}

		private static string GetAssetBundlePlatform(RuntimePlatform platform)
		{
			switch (platform)
			{
				case RuntimePlatform.Android: return "Android";
				case RuntimePlatform.LinuxEditor: return "Linux";
				case RuntimePlatform.LinuxPlayer: return "Linux";
				case RuntimePlatform.WindowsEditor: return "Windows";
				case RuntimePlatform.WindowsPlayer: return "Windows";
				default: throw new ArgumentException(String.Format("Unsupported platform: '{0}'", platform));
			}
		}

		public static int? GameSeed;
	}
}
