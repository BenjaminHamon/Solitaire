namespace BenjaminHamon.Solitaire.Model
{
	/// <summary>The stock is the pile with the leftover cards from the setup, from which the player can draw.</summary>
	public class StockCardPile : CardPile
	{
		public override void Push(Card card)
		{
			base.Push(card);
			card.Visible = false;
		}
	}
}
