using System.Collections.Generic;

namespace BenjaminHamon.Solitaire.Model
{
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
