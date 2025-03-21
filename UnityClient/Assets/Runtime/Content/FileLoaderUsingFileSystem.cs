using System.IO;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Content
{
	public class FileLoaderUsingFileSystem : FileLoader
	{
		public string LoadTextFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}
	}
}
