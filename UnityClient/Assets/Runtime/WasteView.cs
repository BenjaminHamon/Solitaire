using BenjaminHamon.Solitaire.Model;
using System.Linq;
using System;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>The waste is the card pile where cards drawn from the stock are put.</summary>
	public class WasteView : CardPileView
	{
		public new Waste Model
		{
			get { return (Waste)ModelAsObject; }
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
