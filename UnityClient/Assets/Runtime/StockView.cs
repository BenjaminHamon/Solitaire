using BenjaminHamon.Solitaire.Model;
using System;
using System.Linq;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>The stock is the pile with the leftover cards from the setup, from which the player can draw.</summary>
	public class StockView : CardPileView
	{
		public new Stock Model
		{
			get { return (Stock)ModelAsObject; }
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

		public void OnMouseUp()
		{
			if (cardCollection.Any())
			{
				CardView drawnCardView = cardCollection.First();
				Card drawnCard = Model.Draw();

				if (drawnCardView.Model != drawnCard)
				{
					throw new ApplicationException("CardView does not match Card");
				}
			}
			else
			{
				Model.ResetFromWaste();
			}
		}
	}
}
