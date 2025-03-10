using System.IO;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Serialization
{
	public class UnityJsonSerializer : Serializer
	{
		public UnityJsonSerializer(bool prettyPrint)
		{
			this.prettyPrint = prettyPrint;
		}

		private readonly bool prettyPrint;

		public string GetFileExtension()
		{
			return ".json";
		}

		public void SerializeToStream(Stream stream, object value)
		{
			using (StreamWriter streamWriter = new StreamWriter(stream))
				streamWriter.WriteLine(JsonUtility.ToJson(value, prettyPrint));
		}

		public T DeserializeFromStream<T>(Stream stream)
		{
			using (StreamReader streamReader = new StreamReader(stream))
				return JsonUtility.FromJson<T>(streamReader.ReadToEnd());
		}

		public void SerializeToFile(string path, object value)
		{
			using (FileStream fileStream = File.OpenWrite(path + ".tmp"))
				SerializeToStream(fileStream, value);

			File.Delete(path);
			File.Move(path + ".tmp", path);
		}

		public T DeserializeFromFile<T>(string path)
		{
			using (FileStream fileStream = File.OpenRead(path))
				return DeserializeFromStream<T>(fileStream);
		}

		public string SerializeToString(object value)
		{
			return JsonUtility.ToJson(value, prettyPrint);
		}

		public T DeserializeFromString<T>(string serializedValue)
		{
			return JsonUtility.FromJson<T>(serializedValue);
		}
	}
}
