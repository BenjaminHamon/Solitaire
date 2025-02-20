using System;
using UnityEditor;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
    public static class ConvertUnityEnum
    {
		public static BuildTargetGroup ConvertGenericPlatformToUnityBuildTargetGroup(string platform)
		{
			switch (platform)
			{
				case "Android": return BuildTargetGroup.Android;
				case "Linux": return BuildTargetGroup.Standalone;
				case "Windows": return BuildTargetGroup.Standalone;
				default: throw new ArgumentException(String.Format("Unsupported platform: '{0}'", platform));
			}
		}

		public static BuildTarget ConvertGenericPlatformToUnityBuildTarget(string platform)
		{
			switch (platform)
			{
				case "Android": return BuildTarget.Android;
				case "Linux": return BuildTarget.StandaloneLinux64;
				case "Windows": return BuildTarget.StandaloneWindows64;
				default: throw new ArgumentException(String.Format("Unsupported platform: '{0}'", platform));
			}
		}
	}
}
