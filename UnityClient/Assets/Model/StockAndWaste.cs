using System.Collections.Generic;

namespace BenjaminHamon.Solitaire.Model
{
	/// <summary>
	/// The stock is the pile with the leftover cards from the setup, from which the player can draw.
	/// The waste is the card pile where cards drawn from the stock are put.
	/// </summary>
	public class StockAndWaste
    {
		public StockCardPile StockCardPile { get; } = new StockCardPile();
		public WasteCardPile WasteCardPile { get; } = new WasteCardPile();

		public bool CanDraw()
		{
			return StockCardPile.CanPop();
		}

		public Card Draw()
		{
			Card drawnCard = StockCardPile.Pop();
			WasteCardPile.Push(drawnCard);
			return drawnCard;
		}

		public bool CanReset()
		{
			return (StockCardPile.Peek() == null) && (WasteCardPile.Peek() != null);
		}

		public void Reset()
		{
			List<Card> wasteCards = WasteCardPile.PopAll();

			foreach (Card card in wasteCards)
			{
				StockCardPile.Push(card);
			}
		}
	}
}
