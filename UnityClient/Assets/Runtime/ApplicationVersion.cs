using System;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	[Serializable]
	public class ApplicationVersion
    {
		public ApplicationVersion()
		{
			RevisionDate = DateTime.MinValue.ToUniversalTime();
		}

		public string Identifier { get; set; }
		public string Revision { get; set; }
		public string RevisionShort { get; set; }
		public DateTime RevisionDate { get; set; }
		public string Branch { get; set; }

		public string FullIdentifier
		{
			get
			{
				if (RevisionShort != null)
				{
					return Identifier + "+" + RevisionShort;
				}

				return Identifier;
			}
		}

		public override string ToString()
		{
			return String.Format("ApplicationVersion {0}", FullIdentifier);
		}
	}
}
