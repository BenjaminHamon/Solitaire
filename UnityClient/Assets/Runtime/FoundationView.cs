using BenjaminHamon.Solitaire.Model;
using System.Collections.Generic;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
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
			foreach (FoundationCardPile foundationCardPile in Model.EnumeratePiles())
			{
				AddCardPile(foundationCardPile);
			}

			foreach (FoundationCardPileView foundationCardPileView in PileCollection)
			{
				foundationCardPileView.SetUp(cardViewResolver);
			}
		}

		private List<FoundationCardPileView> PileCollection = new List<FoundationCardPileView>();

		public IEnumerable<FoundationCardPileView> EnumeratePiles()
		{
			return PileCollection.AsReadOnly();
		}

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

		public void AddCardPile(FoundationCardPile foundationCardPile)
		{
			GameObject newGameObject = Instantiate(CardPilePrefab);
			FoundationCardPileView foundationCardPileView = newGameObject.GetComponent<FoundationCardPileView>();
			foundationCardPileView.Model = foundationCardPile;
			newGameObject.transform.SetParent(transform);
			PileCollection.Add(foundationCardPileView);
		}
	}
}
