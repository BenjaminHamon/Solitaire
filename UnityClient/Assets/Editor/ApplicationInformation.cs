using BenjaminHamon.Solitaire.UnityClient.Runtime;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
    public interface ApplicationInformation
    {
		string GetApplicationIdentifier();
		ApplicationVersion GetApplicationVersionForDevelopment();
		ApplicationVersion GetApplicationVersionForRelease();
	}
}
