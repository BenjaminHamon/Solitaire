using BenjaminHamon.Solitaire.Model;
using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>The tableau is the main area, where the player can place cards as sequences of alternating color and reveal hidden cards.</summary>
	public class TableauCardPileView : CardPileView
	{
		public new TableauCardPile Model
		{
			get { return (TableauCardPile)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		public override void SetUp(CardViewResolver cardViewResolver)
		{
			base.SetUp(cardViewResolver);

			cardCollection.FirstOrDefault()?.EnableInteractivity();
		}

		/// <summary>Enumerate the cards under the specified card, including the specified card itself.</summary>
		public IEnumerable<CardView> EnumerateCardsFrom(CardView baseCard)
		{
			foreach (Card card in Model.EnumerateCardsFrom(baseCard.Model))
			{
				yield return cardCollection.Single(x => x.Model == card);
			}
		}

		protected override void HandleCardPopped(Card poppedCard)
		{
			base.HandleCardPopped(poppedCard);

			cardCollection.FirstOrDefault()?.EnableInteractivity();
		}
	}
}
