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

			versionLabel.text = ApplicationStatic.Application.ApplicationVersion.FullIdentifier;

			if (ApplicationStatic.Application.ApplicationVersion.RevisionDate.Kind != DateTimeKind.Utc)
			{
				throw new ApplicationException("RevisionDate should be an UTC datetime");
			}

			versionLabel.text += ScreenExtensions.IsLandscape() ? Environment.NewLine : " - ";
			versionLabel.text += ApplicationStatic.Application.ApplicationVersion.RevisionDate.ToString("dd-MMM-yyyy HH:mm UTC");
		}
	}
}
