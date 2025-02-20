using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	public class AssetBundleBuilder
	{
		public void BuildAllAssetBundles(string platform, string assetBundleDirectory)
		{
			UnityEngine.Debug.LogFormat("[AssetBundleBuilder] Building asset bundles for platform '{0}'", platform);
			UnityEngine.Debug.LogFormat("[AssetBundleBuilder] Writing to '{0}'", assetBundleDirectory);

			BuildTarget unityTarget = ConvertUnityEnum.ConvertGenericPlatformToUnityBuildTarget(platform);
			BuildAssetBundleOptions options = BuildAssetBundleOptions.StrictMode;

			ReflectionEditorContext editorContext = new ReflectionEditorContext();
			editorContext.BuildTarget = unityTarget;

			try
			{
				editorContext.Apply();

				Directory.CreateDirectory(assetBundleDirectory);
				AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(assetBundleDirectory, options, unityTarget);
				BuildResult result = manifest != null ? BuildResult.Succeeded : BuildResult.Failed;
				AssetDatabase.Refresh();

				UnityEngine.Debug.LogFormat("[AssetBundleBuilder] Build completed with status '{0}'", result);

				if (manifest == null)
					throw new Exception("Build failed");
			}
			finally
			{
				editorContext.Revert();
			}
		}
	}
}
