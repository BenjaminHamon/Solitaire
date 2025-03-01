using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	/// <summary>The waste is the card pile where cards drawn from the stock are put.</summary>
	public class Waste : CardPile
	{
		public override void Push(Card card)
		{
			base.Push(card);
			card.Visible = true;
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
