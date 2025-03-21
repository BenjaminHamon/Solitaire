using BenjaminHamon.DevelopmentToolkit.Toolkit.Processes;
using BenjaminHamon.DevelopmentToolkit.Toolkit.RevisionControl;
using BenjaminHamon.Solitaire.UnityClient.Runtime;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Serialization;
using System.IO;
using UnityEditor;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	/// <summary>Main class while in the Unity editor, exposing edit mode features.</summary>
	public class UnityEditorApplication
    {
		public UnityEditorApplication()
		{
			ApplicationInformation = new ApplicationInformationImplementation();
			Serializer = UnityApplicationFactory.CreateSerializer();
		}

		private readonly ApplicationInformation ApplicationInformation;
		private readonly Serializer Serializer;

		public void WriteApplicationVersionToStreamingAssets()
		{
			try
			{
				ApplicationVersion applicationVersion = ApplicationInformation.GetApplicationVersionForDevelopment();
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

		private string GetApplicationVersionFilePath()
		{
			return Path.Combine(UnityEngine.Application.streamingAssetsPath, "ApplicationVersion" + Serializer.GetFileExtension());
		}
	}
}
