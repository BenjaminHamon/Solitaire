using BenjaminHamon.Solitaire.Model;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using System.IO;
using System;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>Main class for the Unity client, wrapping the underlying Solitaire application and exposing Unity features.</summary>
    public class UnityApplication
	{
		public UnityApplication()
		{
			InternalApplication = new Model.Application();
			AssetLoader = CreateAssetLoader();
		}

		private readonly Model.Application InternalApplication;
		public AssetLoader<UnityEngine.Object> AssetLoader { get; }
		public int? GameSeed { get; set; }

		public Game NewGame(GameConfiguration configuration, int seed)
		{
			return InternalApplication.NewGame(configuration, seed);
		}

		public void EndGame()
		{
			InternalApplication.EndGame();
		}

		private static AssetLoader<UnityEngine.Object> CreateAssetLoader()
		{
#if UNITY_EDITOR
			bool UseAssetBundlesInEditor = false;

			if (UnityEngine.Application.isEditor)
			{
				if (UseAssetBundlesInEditor)
				{
					string AssetBundlesPath = Path.Combine("..", "Artifacts", "AssetBundles", GetAssetBundlePlatform(UnityEngine.Application.platform));
					return new AssetLoaderUsingBundles(AssetBundlesPath);
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
	}
}
