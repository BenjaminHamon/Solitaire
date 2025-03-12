using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityExtensions.Runtime
{
    public static class ScreenExtensions
    {
		/// <summary>Get a better screen height value, notably for mobile platforms.</summary>
		public static int GetRealHeight()
		{
			// Use greatly reduced size for mobiles since the screen values seem way too big for what is actually rendered
			return Application.isMobilePlatform ? Screen.height / 5 : Screen.height;
		}

		/// <summary>Get a better screen width value, notably for mobile platforms.</summary>
		public static int GetRealWidth()
		{
			// Use greatly reduced size for mobiles since the screen values seem way too big for what is actually rendered
			return Application.isMobilePlatform ? Screen.width / 5 : Screen.width;
		}

		/// <summary>Returns true if the screen size corresponds to landscape mode (wider than tall), false otherwise.</summary>
		public static bool IsLandscape()
		{
			return (float) GetRealWidth() / GetRealHeight() >= 1;
		}

		/// <summary>Returns true if the screen size corresponds to portrait mode (taller than wide), false otherwise.</summary>
		public static bool IsPortrait()
		{
			return (float)GetRealWidth() / GetRealHeight() <= 1;
		}
	}
}
