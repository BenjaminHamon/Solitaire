using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using BenjaminHamon.Solitaire.UnityExtensions.Runtime;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	[ExecuteAlways]
    public class MainMenuScene : MonoBehaviour
	{
		[SerializeField]
		private MainMenu MainMenu;
		[SerializeField]
		private VersionInformationView VersionInformation;

		private float lastUpdateScreenHeight = 0;
		private float lastUpdateScreenWidth = 0;

		public void Start()
		{
			if (Application.IsPlaying(gameObject))
			{
				ApplicationStatic.Application.AssetLoader.LoadBundle(AssetBundleNames.Interface);
			}

			lastUpdateScreenHeight = Screen.height;
			lastUpdateScreenWidth = Screen.width;

			ApplyStyles();

		}

		public void Update()
		{
			if ((lastUpdateScreenWidth != Screen.width) || (lastUpdateScreenHeight != Screen.height))
			{
				UnityEngine.Debug.LogFormat(this, "[MainMenuScene] Screen size changed");

				ApplyStyles();

				lastUpdateScreenHeight = Screen.height;
				lastUpdateScreenWidth = Screen.width;
			}

		}

		public void OnDestroy()
		{
			if (Application.IsPlaying(gameObject))
			{
				ApplicationStatic.Application.AssetLoader.UnloadBundle(AssetBundleNames.Interface);
			}
		}

		private void ApplyStyles()
		{
			AssetLoader<UnityEngine.Object> assetLoader
				= Application.IsPlaying(gameObject) ? ApplicationStatic.Application.AssetLoader : UnityApplicationFactory.CreateAssetLoader();

			ApplicationViewResources applicationViewResources
				= Application.IsPlaying(gameObject) ? ApplicationStatic.Application.ViewResources : UnityApplicationFactory.CreateApplicationViewResources();

			int screenHeight = ScreenExtensions.GetRealHeight();
			int screenWidth = ScreenExtensions.GetRealWidth();

			UnityEngine.Debug.LogFormat(this, "[MainMenuScene] Applying styles for screen {0}x{1} (Actual: {2}x{3})",
				screenWidth, screenHeight, Screen.width, Screen.height);

			MainMenu.ApplyStyles(applicationViewResources.GetStyleCollectionForMain(assetLoader));
			VersionInformation.ApplyStyles(applicationViewResources.GetStyleCollectionForVersionInformation(assetLoader));
		}
	}
}
