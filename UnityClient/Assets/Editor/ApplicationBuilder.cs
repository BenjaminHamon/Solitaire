using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	public class ApplicationBuilder
	{
		public void BuildApplicationPackage(string platform, string configuration, string assetBundleDirectory, string packageDirectory)
		{
			BuildTargetGroup unityTargetGroup = ConvertUnityEnum.ConvertGenericPlatformToUnityBuildTargetGroup(platform);
			BuildTarget unityTarget = ConvertUnityEnum.ConvertGenericPlatformToUnityBuildTarget(platform);

			UnityEngine.Debug.LogFormat("[PackageBuilder] Building application package for platform '{0}' with configuration '{1}'", platform, configuration);
			UnityEngine.Debug.LogFormat("[PackageBuilder] Writing package files to '{0}'", packageDirectory);

			BuildOptions options = GetOptions(configuration);
			string packagePath = GetPackagePath(unityTarget, packageDirectory, Application.ApplicationFullName);
			List<string> sceneCollection = new List<string>() { "Assets/MenuScene.unity", "Assets/GameScene.unity" };

			BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
			{
				targetGroup = unityTargetGroup,
				target = unityTarget,
				options = options,
				locationPathName = packagePath,
				scenes = sceneCollection.ToArray(),
			};

			ReflectionEditorContext editorContext = new ReflectionEditorContext();
			editorContext.BuildTarget = unityTarget;

			try
			{
				editorContext.Apply();

				ClearStreamingAssets();
				CopyAssetBundlesToStreamingAssets(assetBundleDirectory);
				BuildReport buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
				ClearStreamingAssets();

				UnityEngine.Debug.LogFormat("[PackageBuilder] Build completed with status '{0}' ({1} errors, {2} warnings)",
					buildReport.summary.result, buildReport.summary.totalErrors, buildReport.summary.totalWarnings);

				if (buildReport.summary.result == BuildResult.Failed)
					throw new Exception("Build failed");
			}
			finally
			{
				editorContext.Revert();
			}
		}

		private void ClearStreamingAssets()
		{
			if (Directory.Exists(UnityEngine.Application.streamingAssetsPath))
				Directory.Delete(UnityEngine.Application.streamingAssetsPath, true);
			if (File.Exists(UnityEngine.Application.streamingAssetsPath + ".meta"))
				File.Delete(UnityEngine.Application.streamingAssetsPath + ".meta");
		}

		private void CopyAssetBundlesToStreamingAssets(string sourceDirectory)
		{
			string outputDirectory = Path.Combine(UnityEngine.Application.streamingAssetsPath, "AssetBundles");

			UnityEngine.Debug.LogFormat("[PackageBuilder] Copying asset bundles ({0} => {1}", sourceDirectory, outputDirectory);

			List<string> allFiles = Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories)
				.Where(filePath => Path.GetExtension(filePath) != ".meta")
				.Select(filePath => Regex.Replace(filePath, "^" + Regex.Escape(sourceDirectory + Path.DirectorySeparatorChar), ""))
				.ToList();

			foreach (string sourcePath in allFiles)
			{
				string source = Path.Combine(sourceDirectory, sourcePath);
				string destination = Path.Combine(outputDirectory, sourcePath);

				Directory.CreateDirectory(Path.GetDirectoryName(destination));
				File.Copy(source, destination);
			}
		}

		private BuildOptions GetOptions(string configuration)
		{
			switch (configuration)
			{
				case "Debug": return BuildOptions.StrictMode | BuildOptions.Development | BuildOptions.AllowDebugging;
				case "Release": return BuildOptions.StrictMode;
				default: throw new ArgumentException(String.Format("Unknown configuration: '{0}'", configuration));
			}
		}

		private string GetPackagePath(BuildTarget platform, string path, string application)
		{
			switch (platform)
			{
				case BuildTarget.Android: return Path.Combine(path, application + ".apk");
				case BuildTarget.StandaloneLinux64: return Path.Combine(path, application);
				case BuildTarget.StandaloneWindows64: return Path.Combine(path, application + ".exe");
				default: throw new ArgumentException(String.Format("Unsupported platform: '{0}'", platform));
			}
		}
	}
}
