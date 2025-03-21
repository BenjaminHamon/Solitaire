using BenjaminHamon.Solitaire.UnityExtensions.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Views
{
    public class VersionInformationView : ViewElement
    {
		[SerializeField]
		private UIDocument UIDocument;

		public void ApplyStyles(IEnumerable<StyleSheet> styleSheetCollection)
		{
			UIDocument.ApplyStyles(styleSheetCollection);

			if (Application.IsPlaying(gameObject))
			{
				UpdateVersionLabel();
			}
		}

		private void UpdateVersionLabel()
		{
			Label versionLabel = UIDocument.rootVisualElement.Query<Label>("VersionLabel");
			ApplicationVersion applicationVersion = ApplicationStatic.Application.ApplicationVersion;

			versionLabel.text = applicationVersion.FullIdentifier;

			if (applicationVersion.RevisionDate != null)
			{
				if (applicationVersion.RevisionDate?.Kind != DateTimeKind.Utc)
				{
					throw new ApplicationException("RevisionDate should be an UTC datetime");
				}

				bool useMultiline = ScreenExtensions.IsLandscape() || (ScreenExtensions.GetRealWidth() < 400);

				versionLabel.text += useMultiline ? Environment.NewLine : " - ";
				versionLabel.text += applicationVersion.RevisionDate?.ToString("dd-MMM-yyyy HH:mm UTC");
			}
		}
	}
}
