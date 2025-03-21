using System.Collections.Generic;
using UnityEngine.UIElements;

namespace BenjaminHamon.Solitaire.UnityExtensions.Runtime
{
    public static class UIDocumentExtensions
    {
		public static void ApplyStyles(this UIDocument document, IEnumerable<StyleSheet> styleSheetCollection)
		{
			document.rootVisualElement.styleSheets.Clear();

			foreach (StyleSheet styleSheet in styleSheetCollection)
			{
				document.rootVisualElement.styleSheets.Add(styleSheet);
			}
		}
	}
}
