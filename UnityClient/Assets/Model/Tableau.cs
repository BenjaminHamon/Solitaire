using System.Collections.Generic;

namespace BenjaminHamon.Solitaire.Model
{
	/// <summary>The tableau is the main area, where the player can place cards as sequences of alternating color and reveal hidden cards.</summary>
	public class Tableau
    {
		private List<TableauCardPile> pileCollection = new List<TableauCardPile>();

		public TableauCardPile AddCardPile(int cardMaxNumber)
		{
			TableauCardPile tableauPile = new TableauCardPile(cardMaxNumber);
			pileCollection.Add(tableauPile);
			return tableauPile;
		}

		public IEnumerable<TableauCardPile> EnumeratePiles()
		{
			return pileCollection.AsReadOnly();
		}
	}
}
