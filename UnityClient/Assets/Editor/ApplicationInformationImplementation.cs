using BenjaminHamon.DevelopmentToolkit.Toolkit.RevisionControl;
using BenjaminHamon.Solitaire.UnityClient.Runtime;
using System;

namespace BenjaminHamon.Solitaire.UnityClient.Editor
{
	public class ApplicationInformationImplementation : ApplicationInformation
	{
		public ApplicationInformationImplementation()
		{
			RevisionControlClient = new GitClient();
		}

		private readonly RevisionControlClient RevisionControlClient;

		public string GetApplicationIdentifier()
		{
			return "BenjaminHamon.Solitaire";
		}

		public ApplicationVersion GetApplicationVersionForDevelopment()
		{
			ApplicationVersion applicationVersion = new ApplicationVersion() { Identifier = "Development" };

			applicationVersion.Revision = RevisionControlClient.GenerateRevisionWithLocalChanges();
			applicationVersion.RevisionShort = RevisionControlClient.ConvertRevisionToRevisionShort(applicationVersion.Revision);
			applicationVersion.RevisionDate = DateTime.UtcNow;
			applicationVersion.Branch = RevisionControlClient.GetCurrentBranch();

			return applicationVersion;
		}

		public ApplicationVersion GetApplicationVersionForRelease()
		{
			throw new NotImplementedException();

			//// Not implemented: retrieve version identifier

			//ApplicationVersion applicationVersion = new ApplicationVersion() { Identifier = "FIXME" };

			//// Not implemented: ensure no local changes

			//applicationVersion.Revision = RevisionControlClient.GetCurrentRevision();
			//applicationVersion.RevisionShort = RevisionControlClient.ConvertRevisionToRevisionShort(applicationVersion.Revision);
			//applicationVersion.RevisionDate = RevisionControlClient.GetRevisionDate(applicationVersion.Revision);
			//applicationVersion.Branch = RevisionControlClient.GetCurrentBranch();

			//return applicationVersion;
		}
	}
}
