using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	public static class EditorCommandHelpers
	{
		public static void ConfigureLogging()
		{
			UnityEngine.Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
		}

		public static Dictionary<string, List<string>> ConvertRawArgumentsToDictionary(IList<string> rawArguments = null)
		{
			if (rawArguments == null)
			{
				rawArguments = Environment.GetCommandLineArgs();
			}

			string currentArgumentKey = null;
			Dictionary<string, List<string>> argumentAsDictionary = new Dictionary<string, List<string>>();

			foreach (string argument in rawArguments)
			{
				if (argument.StartsWith("-"))
				{
					currentArgumentKey = argument.Substring(1);
					argumentAsDictionary.Add(currentArgumentKey, new List<string>());
				}
				else
				{
					if (currentArgumentKey != null)
					{
						argumentAsDictionary[currentArgumentKey].Add(argument);
					}
				}
			}

			return argumentAsDictionary;
		}

		public static string GetCommandName(Dictionary<string, List<string>> arguments)
		{
			return arguments["executeMethodCommand"].First();
		}

		public static Dictionary<string, string> GetCommandArguments(Dictionary<string, List<string>> arguments)
		{
			List<string> rawArguments = arguments["executeMethodArguments"];
			Dictionary<string, string> commandArguments = new Dictionary<string, string>();

			foreach (string argument in rawArguments)
			{
				IList<string> keyValuePair = argument.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
				if (keyValuePair.Count != 2)
					throw new ArgumentException(String.Format("Invalid argument: '{0}'", argument));
				commandArguments.Add(keyValuePair[0], keyValuePair[1]);
			}

			return commandArguments;
		}

		public static TValue ParseArgument<TValue>(Dictionary<string, string> arguments, string key)
		{
			if (arguments.ContainsKey(key) == false)
				throw new ArgumentException(String.Format("Missing argument: '{0}'", key));
			return (TValue) Convert.ChangeType(arguments[key], typeof(TValue));
		}
	}
}
