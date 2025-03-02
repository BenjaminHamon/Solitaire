using System;
using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	/// <summary>The foundations are the piles where the player must stack cards as same type sequences to achieve victory.</summary>
	public class Foundation
	{
		public event Action AllPilesCompleted;

		private List<FoundationCardPile> pileCollection = new List<FoundationCardPile>();

		public FoundationCardPile AddCardPile(int cardMaxNumber)
		{
			FoundationCardPile foundationPile = new FoundationCardPile(cardMaxNumber);
			pileCollection.Add(foundationPile);
			return foundationPile;
		}

		public IEnumerable<FoundationCardPile> EnumeratePiles()
		{
			return pileCollection.AsReadOnly();
		}

		public void EnableEventListeners()
		{
			foreach (FoundationCardPile foundationCardPile in pileCollection)
			{
				foundationCardPile.Completed += HandlePileCompleted;
			}
		}

		public void DisableEventListeners()
		{
			foreach (FoundationCardPile foundationCardPile in pileCollection)
			{
				foundationCardPile.Completed -= HandlePileCompleted;
			}
		}

		public bool TryPush(Card card)
		{
			foreach (FoundationCardPile foundationCardPile in pileCollection)
			{
				if (foundationCardPile.CanPush(card))
				{
					if (card.Parent != null)
					{
						card.Parent.Pop();
					}

					foundationCardPile.Push(card);

					return true;
				}
			}

			return false;
		}

		private void HandlePileCompleted()
		{
			if (AreAllPilesCompleted())
			{
				AllPilesCompleted?.Invoke();
			}
		}

		public bool AreAllPilesCompleted()
		{
			return pileCollection.All(cardPile => cardPile.IsComplete);
		}
	}
}
