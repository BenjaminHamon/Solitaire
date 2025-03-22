using System;
using System.Collections.Generic;
using System.Linq;

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

		public bool TryMoveCardPile(TableauCardPile fromCardPile, TableauCardPile toCardPile)
		{
			List<Card> allCardsToMove = null;

			foreach (Card card in fromCardPile.EnumerateCards())
			{
				if (card.Visible == false)
					break;

				if (toCardPile.CanPush(card))
				{
					allCardsToMove = fromCardPile.EnumerateCardsFrom(card).ToList();
					break;
				}
			}

			if (allCardsToMove != null)
			{
				foreach (Card cardToMove in allCardsToMove.Reverse<Card>())
				{
					if (cardToMove.Parent.CanPop() == false)
					{
						throw new InvalidOperationException("Card to move cannot be popped");
					}

					Card poppedCard = fromCardPile.Pop();

					if (poppedCard != cardToMove)
					{
						throw new InvalidOperationException("Popped card is not as expected");
					}
				}

				foreach (Card cardToMove in allCardsToMove)
				{
					if (toCardPile.CanPush(cardToMove) == false)
					{
						throw new InvalidOperationException("Card to move cannot be pushed");
					}

					toCardPile.Push(cardToMove);
				}

				return true;
			}

			return false;
		}
	}
}
