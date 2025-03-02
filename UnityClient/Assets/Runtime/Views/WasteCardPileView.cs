using BenjaminHamon.Solitaire.Model;
using System.Linq;
using System;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Views
{
	/// <summary>The waste is the card pile where cards drawn from the stock are put.</summary>
	public class WasteCardPileView : CardPileView
	{
		public new WasteCardPile Model
		{
			get { return (WasteCardPile)ModelAsObject; }
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

			pushedCardView.EnableInteractivity();
		}
	}
}
