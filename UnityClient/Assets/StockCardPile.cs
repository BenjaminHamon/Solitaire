using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.UnityClient
{
	/// <summary>The stock is the pile with the leftover cards from the setup, from which the player can draw.</summary>
	public class StockCardPile : CardPile
	{
		public WasteCardPile Waste;

		public void OnMouseUp()
		{
			if (Cards.Any())
			{
				Draw();
			}
			else
			{
				ResetFromWaste();
			}
		}

		private void Draw()
		{
			Card drawnCard = Cards.Pop();
			Waste.Push(drawnCard);
		}

		private void ResetFromWaste()
		{
			IEnumerable<Card> wasteCards = Waste.PopAll();

			foreach (Card card in wasteCards)
			{
				Push(card);
			}
		}

		public override void Push(Card card)
		{
			base.Push(card);

			card.Visible = false;
			card.Collider.enabled = false;
		}
	}
}
