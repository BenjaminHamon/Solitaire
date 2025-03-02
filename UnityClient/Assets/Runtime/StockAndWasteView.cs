using BenjaminHamon.Solitaire.Model;
using System.Collections.Generic;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
    public class StockAndWasteView : ViewElement
    {
		public StockAndWaste Model
		{
			get { return (StockAndWaste)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		[SerializeField]
		private StockCardPileView Stock;
		[SerializeField]
		private WasteCardPileView Waste;

		public void SetUp(CardViewResolver cardViewResolver)
		{
			Stock.Model = Model.StockCardPile;
			Waste.Model = Model.WasteCardPile;

			Stock.SetUp(cardViewResolver);
			Waste.SetUp(cardViewResolver);
		}

		public IEnumerable<CardView> EnumerateCards()
		{
			foreach (CardView card in Stock.EnumerateCards())
			{
				yield return card;
			}

			foreach (CardView card in Waste.EnumerateCards())
			{
				yield return card;
			}
		}

		public void OnMouseUp()
		{
			if (Model.CanDraw())
			{
				Model.Draw();
			}
			else if (Model.CanReset())
			{
				Model.Reset();
			}
		}
	}
}
