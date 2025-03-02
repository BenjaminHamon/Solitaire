using BenjaminHamon.Solitaire.Model;
using System;
using System.Linq;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>The stock is the pile with the leftover cards from the setup, from which the player can draw.</summary>
	public class StockCardPileView : CardPileView
	{
		public new StockCardPile Model
		{
			get { return (StockCardPile)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		protected override void HandleCardPushed(Card pushedCard)
		{
			base.HandleCardPushed(pushedCard);

			CardView pushedCardView = cardCollection.First();

			if (pushedCardView.Model != pushedCard)
			{
				throw new ApplicationException("CardView does not match Card");
			}

			pushedCardView.DisableInteractivity();
		}
	}
}
