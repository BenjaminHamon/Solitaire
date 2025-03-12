using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using BenjaminHamon.Solitaire.UnityExtensions.Runtime;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Views
{
    public class ApplicationViewResources
    {
		public string DateFormatShort { get { return "dd-MMM-yyyy"; } }

		public List<StyleSheet> GetStyleCollectionForMain(AssetLoader<UnityEngine.Object> assetLoader)
		{
			int screenHeight = ScreenExtensions.GetRealHeight();
			int screenWidth = ScreenExtensions.GetRealWidth();
			float screenRatio = (float) screenWidth / screenHeight;

			List<StyleSheet> styleSheetCollection = new List<StyleSheet>();

			styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(AssetBundleNames.Interface, "Interface/Main/Generic.uss"));

			if (screenRatio > 1)
			{
				if (screenWidth > 1000)
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/Main/LandscapeHighResolutionStyles.uss"));
				}
				else
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/Main/GenericLowResolution.uss"));
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/Main/LandscapeLowResolutionStyles.uss"));
				}
			}
			else
			{
				if (screenHeight > 1000)
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/Main/PortraitHighResolutionStyles.uss"));
				}
				else
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/Main/GenericLowResolution.uss"));
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/Main/PortraitLowResolutionStyles.uss"));
				}
			}

			return styleSheetCollection;
		}

		public List<StyleSheet> GetStyleCollectionForVersionInformation(AssetLoader<UnityEngine.Object> assetLoader)
		{
			int screenHeight = ScreenExtensions.GetRealHeight();
			int screenWidth = ScreenExtensions.GetRealWidth();
			float screenRatio = (float)screenWidth / screenHeight;

			List<StyleSheet> styleSheetCollection = new List<StyleSheet>();

			styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(AssetBundleNames.Interface, "Interface/VersionInformation/Generic.uss"));

			if (screenRatio > 1)
			{
				if (screenWidth > 1000)
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/VersionInformation/LandscapeHighResolutionStyles.uss"));
				}
				else
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/VersionInformation/GenericLowResolution.uss"));
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/VersionInformation/LandscapeLowResolutionStyles.uss"));
				}
			}
			else
			{
				if (screenHeight > 1000)
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/VersionInformation/PortraitHighResolutionStyles.uss"));
				}
				else
				{
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/VersionInformation/GenericLowResolution.uss"));
					styleSheetCollection.Add(assetLoader.LoadByPath<StyleSheet>(
						AssetBundleNames.Interface, "Interface/VersionInformation/PortraitLowResolutionStyles.uss"));
				}
			}

			return styleSheetCollection;
		}
	}
}
