namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>Global static class to access the actual application from a Unity component.</summary>
	public static class ApplicationStatic
    {
		static ApplicationStatic()
		{
			UnityEngine.Debug.LogFormat("[Application] Static initialization");

			Application = new UnityApplication();
			Application.LoadApplicationInformation();

			UnityEngine.Debug.LogFormat("[Application] Static initialization complete");
			UnityEngine.Debug.LogFormat("[Application] Starting (Version: '{0}')", Application.ApplicationVersion?.FullIdentifier ?? "__null__");
		}

		public static readonly UnityApplication Application;
	}
}
