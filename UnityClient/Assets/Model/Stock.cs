using System.Collections.Generic;

namespace BenjaminHamon.Solitaire.Model
{
	/// <summary>The stock is the pile with the leftover cards from the setup, from which the player can draw.</summary>
	public class Stock : CardPile
	{
		public Stock(Waste waste)
		{
			this.waste = waste;
		}

		private readonly Waste waste;

		public Card Draw()
		{
			Card drawnCard = Pop();
			waste.Push(drawnCard);
			return drawnCard;
		}

		public void ResetFromWaste()
		{
			List<Card> wasteCards = waste.PopAll();

			foreach (Card card in wasteCards)
			{
				Push(card);
			}
		}

		public override void Push(Card card)
		{
			base.Push(card);
			card.Visible = false;
		}
	}
}
