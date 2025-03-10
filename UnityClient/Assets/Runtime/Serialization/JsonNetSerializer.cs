using Newtonsoft.Json;
using System.IO;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Serialization
{
	public class JsonNetSerializer : Serializer
	{
		public JsonNetSerializer(JsonSerializer implementation)
		{
			this.implementation = implementation;
		}

		private readonly JsonSerializer implementation;

		public string GetFileExtension()
		{
			return ".json";
		}

		public void SerializeToStream(Stream stream, object value)
		{
			using (StreamWriter streamWriter = new StreamWriter(stream))
			using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
				implementation.Serialize(jsonWriter, value);
		}

		public T DeserializeFromStream<T>(Stream stream)
		{
			using (StreamReader streamReader = new StreamReader(stream))
			using (JsonReader jsonReader = new JsonTextReader(streamReader))
				return implementation.Deserialize<T>(jsonReader);
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
			using (StringWriter stringWriter = new StringWriter())
			{
				using (JsonWriter jsonWriter = new JsonTextWriter(stringWriter))
					implementation.Serialize(jsonWriter, value);

				return stringWriter.ToString();
			}
		}

		public T DeserializeFromString<T>(string serializedValue)
		{
			using (StringReader stringReader = new StringReader(serializedValue))
			using (JsonReader jsonReader = new JsonTextReader(stringReader))
				return implementation.Deserialize<T>(jsonReader);
		}
	}
}
