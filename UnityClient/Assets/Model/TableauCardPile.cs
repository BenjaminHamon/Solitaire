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
			Card topCard = cardCollection.FirstOrDefault();
			bool canPushAsFirstCard = (topCard == null) && (card.Number == cardMaxNumber);
			bool canPushAsNextCard = (topCard != null) && topCard.Visible && (topCard.Type.ToColor() != card.Type.ToColor()) && (topCard.Number == card.Number + 1);

			return canPushAsFirstCard || canPushAsNextCard;
		}

		/// <summary>Enumerate the cards under the specified card, including the specified card itself.</summary>
		public IEnumerable<Card> EnumerateCardsFrom(Card baseCard)
		{
			yield return baseCard;

			foreach (Card card in cardCollection.TakeWhile(card => card != baseCard).Reverse())
			{
				yield return card;
			}
		}
	}
}
