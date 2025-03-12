using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
    public static class UnityApplicationFactory
    {
		public static Serializer CreateSerializer()
		{
			JsonSerializer serializationImplementation = new JsonSerializer();
			serializationImplementation.Converters.Add(new StringEnumConverter());
			serializationImplementation.Formatting = Formatting.Indented;

			return new JsonNetSerializer(serializationImplementation);
		}

		public static AssetLoader<UnityEngine.Object> CreateAssetLoader()
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

		public static ApplicationViewResources CreateApplicationViewResources()
		{
			return new ApplicationViewResources();
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
