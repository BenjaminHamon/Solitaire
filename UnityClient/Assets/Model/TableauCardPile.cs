using System;
using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	public class TableauCardPile : CardPile
	{
		public TableauCardPile(int cardMaxNumber)
		{
			this.cardMaxNumber = cardMaxNumber;
		}

		private readonly int cardMaxNumber;

		public override bool CanPush(Card card)
		{
			if (card.Visible == false)
				return false;

			Card topCard = cardCollection.FirstOrDefault();
			bool canPushAsFirstCard = (topCard == null) && (card.Number == cardMaxNumber);
			bool canPushAsNextCard = (topCard != null) && topCard.Visible && (topCard.Type.ToColor() != card.Type.ToColor()) && (topCard.Number == card.Number + 1);

			return canPushAsFirstCard || canPushAsNextCard;
		}

		public override bool CanPop()
		{
			Card topCardOrNull = Peek();
			return topCardOrNull != null && topCardOrNull.Visible;
		}

		/// <summary>Enumerate the cards under the specified card, including the specified card itself.</summary>
		public IEnumerable<Card> EnumerateCardsFrom(Card baseCard)
		{
			if (baseCard.Visible == false)
			{
				throw new InvalidOperationException("Base card must be visible");
			}

			yield return baseCard;

			foreach (Card card in cardCollection.TakeWhile(card => card != baseCard).Reverse())
			{
				yield return card;
			}
		}
	}
}
