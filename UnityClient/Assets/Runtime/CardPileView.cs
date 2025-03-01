using BenjaminHamon.Solitaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	public class CardPileView : ViewElement
	{
		public CardPile Model
		{
			get { return (CardPile)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		protected CardViewResolver cardViewResolver;

		public virtual void SetUp(CardViewResolver cardViewResolver)
		{
			this.cardViewResolver = cardViewResolver;

			foreach (Card card in Model.EnumerateCards().Reverse())
			{
				CardView cardView = InstantiateCard(card);
				cardCollection.Push(cardView);
				UpdateCardTransformAfterPush(cardView);
			}
		}

		protected Stack<CardView> cardCollection = new Stack<CardView>();

		public IEnumerable<CardView> EnumerateCards()
		{
			return cardCollection.ToList().AsReadOnly();
		}

		public virtual void Start()
		{
			Model.CardPushed += HandleCardPushed;
			Model.CardPopped += HandleCardPopped;
		}

		public virtual void OnEnable()
		{
			if (Model != null)
			{
				Model.CardPushed += HandleCardPushed;
				Model.CardPopped += HandleCardPopped;
			}
		}

		public virtual void OnDisable()
		{
			if (Model != null)
			{
				Model.CardPushed -= HandleCardPushed;
				Model.CardPopped -= HandleCardPopped;
			}
		}

		private CardView InstantiateCard(Card card)
		{
			GameObject cardPrefab = ApplicationStatic.AssetLoader.LoadByPath<GameObject>(null, "Prefabs/Card.prefab");
			GameObject newGameObject = Instantiate(cardPrefab);
			CardView cardView = newGameObject.GetComponent<CardView>();
			cardView.Model = card;
			cardView.Parent = this;
			cardView.DisableInteractivity();
			return cardView;
		}

		public CardView Peek()
		{
			return cardCollection.FirstOrDefault();
		}

		public virtual bool CanPush(CardView card)
		{
			return Model.CanPush(card.Model);
		}

		public virtual void Push(CardView card)
		{
			Model.Push(card.Model);
		}

		protected virtual void HandleCardPushed(Card pushedCard)
		{
			CardView pushedCardView = cardViewResolver.GetCardView(pushedCard);
			cardCollection.Push(pushedCardView);
			pushedCardView.Parent = this;
			UpdateCardTransformAfterPush(pushedCardView);
		}

		private void UpdateCardTransformAfterPush(CardView pushedCardView)
		{
			Vector3 cardPosition = Vector3.zero;

			if (cardCollection.Count > 1)
			{
				CardView previousTopCard = cardCollection.Skip(1).First();
				cardPosition = previousTopCard.transform.localPosition;
			}

			cardPosition.z -= 0.1f;
			pushedCardView.transform.localPosition = cardPosition;
			pushedCardView.transform.SetParent(transform, false);
		}

		public void Pop()
		{
			Model.Pop();
		}

		protected virtual void HandleCardPopped(Card poppedCard)
		{
			CardView cardView = cardCollection.Pop();
			cardView.Parent = null;

			if (cardView.Model != poppedCard)
			{
				throw new ApplicationException("CardView does not match Card");
			}
		}

		public void ResetDepth()
		{
			float z = -0.1f * cardCollection.Count;

			foreach (CardView card in cardCollection)
			{
				Vector3 cardPosition = card.transform.localPosition;
				cardPosition.z = z;
				card.transform.localPosition = cardPosition;
				z += 0.1f;
			}
		}
	}
}
