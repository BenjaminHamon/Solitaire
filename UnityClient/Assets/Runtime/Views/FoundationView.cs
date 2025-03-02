using BenjaminHamon.Solitaire.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Views
{
	public class FoundationView : ViewElement
	{
		public Foundation Model
		{
			get { return (Foundation)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		[SerializeField]
		private GameObject CardPilePrefab;

		public void SetUp(CardViewResolver cardViewResolver)
		{
			int pileIndex = 0;

			foreach (FoundationCardPile foundationCardPile in Model.EnumeratePiles())
			{
				AddCardPile(foundationCardPile, pileIndex);
				pileIndex += 1;
			}

			foreach (FoundationCardPileView foundationCardPileView in PileCollection)
			{
				foundationCardPileView.SetUp(cardViewResolver);
			}
		}

		private List<FoundationCardPileView> PileCollection = new List<FoundationCardPileView>();

		public IEnumerable<CardView> EnumerateCards()
		{
			foreach (FoundationCardPileView pile in PileCollection)
			{
				foreach (CardView card in pile.EnumerateCards())
				{
					yield return card;
				}
			}
		}

		public bool TryPush(CardView card)
		{
			return Model.TryPush(card.Model);
		}

		public void AddCardPile(FoundationCardPile foundationCardPile, int pileIndex)
		{
			GameObject newGameObject = Instantiate(CardPilePrefab);
			newGameObject.name = String.Format("FoundationCardPile {0}", pileIndex + 1);
			newGameObject.transform.SetParent(transform);

			FoundationCardPileView foundationCardPileView = newGameObject.GetComponent<FoundationCardPileView>();
			foundationCardPileView.Model = foundationCardPile;

			PileCollection.Add(foundationCardPileView);
		}
	}
}
