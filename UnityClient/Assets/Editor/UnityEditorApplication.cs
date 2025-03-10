using BenjaminHamon.DevelopmentToolkit.Toolkit.Processes;
using BenjaminHamon.DevelopmentToolkit.Toolkit.RevisionControl;
using BenjaminHamon.Solitaire.UnityClient.Runtime;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Serialization;
using System;
using System.IO;
using UnityEditor;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	/// <summary>Main class while in the Unity editor, exposing edit mode features.</summary>
	public class UnityEditorApplication
    {
		public UnityEditorApplication()
		{
			RevisionControlClient = new GitClient();
			Serializer = UnityApplicationFactory.CreateSerializer();
		}

		private readonly RevisionControlClient RevisionControlClient;
		private readonly Serializer Serializer;

		public ApplicationVersion GetApplicationVersion()
		{
			ApplicationVersion applicationVersion = new ApplicationVersion() { Identifier = "Development" };

			applicationVersion.Revision = RevisionControlClient.GenerateRevisionWithLocalChanges();
			applicationVersion.RevisionShort = RevisionControlClient.ConvertRevisionToRevisionShort(applicationVersion.Revision);
			applicationVersion.RevisionDate = DateTime.UtcNow;
			applicationVersion.Branch = RevisionControlClient.GetCurrentBranch();

			return applicationVersion;
		}

		public string GetApplicationVersionFilePath()
		{
			return Path.Combine(UnityEngine.Application.streamingAssetsPath, "ApplicationVersion" + Serializer.GetFileExtension());
		}

		public void WriteApplicationVersionToStreamingAssets()
		{
			try
			{
				ApplicationVersion applicationVersion = GetApplicationVersion();
				string applicationVersionFilePath = GetApplicationVersionFilePath();

				Directory.CreateDirectory(Path.GetDirectoryName(applicationVersionFilePath));
				Serializer.SerializeToFile(applicationVersionFilePath, applicationVersion);
			}
			catch (ProcessException exception)
			{
				UnityEngine.Debug.LogError("Failed to write application version to streaming assets");
				UnityEngine.Debug.LogException(exception);
			}
		}

		public void ClearApplicationVersionFromStreamingAssets()
		{
			string applicationVersionFilePath = GetApplicationVersionFilePath();

			if (File.Exists(applicationVersionFilePath))
			{
				File.Delete(applicationVersionFilePath);
				File.Delete(applicationVersionFilePath + ".meta");

				AssetDatabase.Refresh();
			}
		}
	}
}
