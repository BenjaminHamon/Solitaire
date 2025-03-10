using UnityEditor;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
    public static class EditorOnLoad
    {
		[InitializeOnLoadMethod]
		public static void Initialize()
		{
			EditorApplication.playModeStateChanged += HandlePlayModeStateChanged;
		}

		private static void HandlePlayModeStateChanged(PlayModeStateChange newState)
		{
			if (newState == PlayModeStateChange.ExitingEditMode)
			{
				ApplicationStatic.Application.WriteApplicationVersionToStreamingAssets();
			}

			if (newState == PlayModeStateChange.ExitingPlayMode)
			{
				ApplicationStatic.Application.ClearApplicationVersionFromStreamingAssets();
			}
		}
	}
}
