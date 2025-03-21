using System.IO;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Serialization
{
	public interface Serializer
	{
		string GetFileExtension();

		void SerializeToStream(Stream stream, object value);
		T DeserializeFromStream<T>(Stream stream);

		void SerializeToFile(string path, object value);
		T DeserializeFromFile<T>(string path);

		string SerializeToString(object value);
		T DeserializeFromString<T>(string serializedValue);
	}
}
