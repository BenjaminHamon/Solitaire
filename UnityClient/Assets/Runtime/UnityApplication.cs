using BenjaminHamon.Solitaire.Model;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Serialization;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using System.IO;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>Main class for the Unity client, wrapping the underlying Solitaire application and exposing Unity features.</summary>
    public class UnityApplication
	{
		public UnityApplication()
		{
			InternalApplication = new Model.Application();
			FileLoader = UnityApplicationFactory.CreateFileLoader();
			Serializer = UnityApplicationFactory.CreateSerializer();
			AssetLoader = UnityApplicationFactory.CreateAssetLoader();
			ViewResources = UnityApplicationFactory.CreateApplicationViewResources();
		}

		private readonly Model.Application InternalApplication;
		private readonly FileLoader FileLoader;
		private readonly Serializer Serializer;
		public AssetLoader<UnityEngine.Object> AssetLoader { get; }
		public ApplicationViewResources ViewResources { get; }

		public ApplicationVersion ApplicationVersion { get; private set; }

		public int? GameSeed { get; set; }

		public void LoadApplicationInformation()
		{
			ApplicationVersion = new ApplicationVersion() { Identifier = "Unknown" };

			string applicationVersionFileName = "ApplicationVersion" + Serializer.GetFileExtension();
			string applicationVersionFilePath = Path.Combine(UnityEngine.Application.streamingAssetsPath, applicationVersionFileName);

			try
			{
				string applicationVersionText = FileLoader.LoadTextFile(applicationVersionFilePath);
				ApplicationVersion = Serializer.DeserializeFromString<ApplicationVersion>(applicationVersionText);
			}
			catch (IOException exception)
			{
				UnityEngine.Debug.LogError("Failed to load application version");
				UnityEngine.Debug.LogException(exception);
			}
		}

		public Game NewGame(GameConfiguration configuration, int seed)
		{
			return InternalApplication.NewGame(configuration, seed);
		}

		public void EndGame()
		{
			InternalApplication.EndGame();
		}
	}
}
