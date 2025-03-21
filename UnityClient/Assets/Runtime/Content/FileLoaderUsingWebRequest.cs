using UnityEngine.Networking;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Content
{
	public class FileLoaderUsingWebRequest : FileLoader
	{
		public string LoadTextFile(string filePath)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(filePath))
			{
				request.SendWebRequest();
				while (request.isDone == false) { }
				return  request.downloadHandler.text;
			}
		}
	}
}
