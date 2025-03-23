using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	public class WasteCardPile : CardPile
	{
		public override bool CanPush(Card card)
		{
			return true;
		}

		public override void Push(Card card)
		{
			base.Push(card);
			card.Visible = true;
		}

		public override bool CanPop()
		{
			return cardCollection.Any();
		}

		public List<Card> PopAll()
		{
			List<Card> poppedCards = new List<Card>();

			while (cardCollection.Any())
			{
				poppedCards.Add(Pop());
			}

			return poppedCards;
		}
	}
}
