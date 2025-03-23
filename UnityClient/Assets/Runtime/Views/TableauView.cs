using BenjaminHamon.Solitaire.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Views
{
	public class TableauView : ViewElement
	{
		public Tableau Model
		{
			get { return (Tableau)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		[SerializeField]
		private GameObject CardPilePrefab;

		private List<TableauCardPileView> PileCollection = new List<TableauCardPileView>();

		public void SetUp(CardViewResolver cardViewResolver)
		{
			int pileIndex = 0;

			foreach (TableauCardPile tableauCardPile in Model.EnumeratePiles())
			{
				AddCardPile(tableauCardPile, pileIndex);
				pileIndex += 1;
			}

			foreach (TableauCardPileView tableauCardPileView in PileCollection)
			{
				tableauCardPileView.SetUp(cardViewResolver);
			}
		}

		private void AddCardPile(TableauCardPile tableauCardPile, int pileIndex)
		{
			GameObject newGameObject = Instantiate(CardPilePrefab);
			newGameObject.name = String.Format("TableauCardPile {0}", pileIndex + 1);
			newGameObject.transform.SetParent(transform);

			TableauCardPileView tableauCardPileView = newGameObject.GetComponent<TableauCardPileView>();
			tableauCardPileView.Model = tableauCardPile;

			PileCollection.Add(tableauCardPileView);
		}

		public IEnumerable<CardView> EnumerateCards()
		{
			foreach (TableauCardPileView pile in PileCollection)
			{
				foreach (CardView card in pile.EnumerateCards())
				{
					yield return card;
				}
			}
		}
	}
}
