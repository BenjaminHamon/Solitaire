using BenjaminHamon.Solitaire.Model;
using System.Collections.Generic;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
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

		public void SetUp(CardViewResolver cardViewResolver)
		{
			foreach (TableauCardPile tableauCardPile in Model.EnumeratePiles())
			{
				AddCardPile(tableauCardPile);
			}

			foreach (TableauCardPileView tableauCardPileView in PileCollection)
			{
				tableauCardPileView.SetUp(cardViewResolver);
			}
		}

		private List<TableauCardPileView> PileCollection = new List<TableauCardPileView>();

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

		public void AddCardPile(TableauCardPile tableauCardPile)
		{
			GameObject newGameObject = Instantiate(CardPilePrefab);
			TableauCardPileView tableauCardPileView = newGameObject.GetComponent<TableauCardPileView>();
			tableauCardPileView.Model = tableauCardPile;
			newGameObject.transform.SetParent(transform);
			PileCollection.Add(tableauCardPileView);
		}
	}
}
