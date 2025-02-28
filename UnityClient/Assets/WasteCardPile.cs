using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.UnityClient
{
	/// <summary>The waste is the card pile where cards drawn from the stock are put.</summary>
	public class WasteCardPile : CardPile
	{
		public override void Push(Card card)
		{
			base.Push(card);

			card.Visible = true;
			card.Collider.enabled = true;
		}

		public List<Card> PopAll()
		{
			List<Card> poppedCards = new List<Card>(Cards);
			Cards.Clear();
			return poppedCards;
		}
	}
}
