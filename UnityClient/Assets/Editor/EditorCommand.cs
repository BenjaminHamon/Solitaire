using System;
using System.Collections.Generic;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	public static class EditorCommand
	{
		public static void RunEditorCommand()
		{
			EditorCommandHelpers.ConfigureLogging();

			Dictionary<string, List<string>> allArguments = EditorCommandHelpers.ConvertRawArgumentsToDictionary();

			string command = EditorCommandHelpers.GetCommandName(allArguments);
			Dictionary<string, string> commandArguments = EditorCommandHelpers.GetCommandArguments(allArguments);

			UnityEngine.Debug.LogFormat("[EditorCommand] Running command '{0}'", command);

			switch (command)
			{
				case "BuildAssetBundles": BuildAssetBundles(commandArguments); break;
				case "BuildApplication": BuildApplication(commandArguments); break;
				default: throw new Exception(String.Format("Unknown command: '{0}'", command));
			}
		}

		public static void BuildAssetBundles(Dictionary<string, string> arguments)
		{
			string platform = EditorCommandHelpers.ParseArgument<string>(arguments, "platform");
			string assetBundleDirectory = EditorCommandHelpers.ParseArgument<string>(arguments, "assetBundleDirectory");

			AssetBundleBuilder assetBundleBuilder = new AssetBundleBuilder();
			assetBundleBuilder.BuildAllAssetBundles(platform, assetBundleDirectory);
		}

		public static void BuildApplication(Dictionary<string, string> arguments)
		{
			string platform = EditorCommandHelpers.ParseArgument<string>(arguments, "platform");
			string configuration = EditorCommandHelpers.ParseArgument<string>(arguments, "configuration");
			string assetBundleDirectory = EditorCommandHelpers.ParseArgument<string>(arguments, "assetBundleDirectory");
			string packageDirectory = EditorCommandHelpers.ParseArgument<string>(arguments, "packageDirectory");

			ApplicationBuilder applicationBuilder = new ApplicationBuilder();
			applicationBuilder.BuildApplicationPackage(platform, configuration, assetBundleDirectory, packageDirectory);
		}
	}
}
