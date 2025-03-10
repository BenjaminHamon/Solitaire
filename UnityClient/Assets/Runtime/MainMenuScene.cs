using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	[ExecuteAlways]
    public class MainMenuScene : MonoBehaviour
	{
		[SerializeField]
		private MainMenu MainMenuView;

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

			// Use greatly reduced size for mobiles since the screen values seem way too big for what is actually rendered
			float screenHeight = UnityEngine.Application.isMobilePlatform ? Screen.height / 5 : Screen.height;
			float screenWidth = UnityEngine.Application.isMobilePlatform ? Screen.width / 5 : Screen.width;
			float screenRatio = screenWidth / screenHeight;

			UnityEngine.Debug.LogFormat(this, "[MainMenuScene] Applying for screen {0}x{1} (Actual: {2}x{3})",
				screenWidth, screenHeight, Screen.width, Screen.height);

			List<StyleSheet> styleSheetCollection = new List<StyleSheet>();

			styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(AssetBundleNames.Interface, "Interface/Generic.uss"));

			if (screenRatio > 1)
			{
				if (screenWidth > 1000)
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/LandscapeHighResolutionStyles.uss"));
				}
				else
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/GenericLowResolution.uss"));
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/LandscapeLowResolutionStyles.uss"));
				}
			}
			else
			{
				if (screenHeight > 1000)
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/PortraitHighResolutionStyles.uss"));
				}
				else
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/GenericLowResolution.uss"));
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/PortraitLowResolutionStyles.uss"));
				}
			}

			MainMenuView.ApplyStyles(styleSheetCollection);
		}
	}
}
