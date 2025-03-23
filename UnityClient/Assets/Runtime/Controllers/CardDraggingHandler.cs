using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using BenjaminHamon.Solitaire.UnityExtensions.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Controllers
{
	/// <summary>Game object handling a single action of dragging cards.</summary>
    public class CardDraggingHandler : MonoBehaviour
	{
		public Camera Camera;
		public CardView Card;

		private List<CardView> allMovingCards;
		private Vector3 draggingOffset;

		private List<CardView> resolvedMovingCards()
		{
			if (Card.Parent is TableauCardPileView parentAsTableauCardPile)
			{
				return parentAsTableauCardPile.EnumerateCardsFrom(Card).ToList();
			}
			else
			{
				return new List<CardView>() { Card };
			}
		}

		public void Start()
		{
			allMovingCards = resolvedMovingCards();

			foreach (CardView cardView in allMovingCards)
			{
				// Disable interactivity for dragged cards to detect the card underneath on drop.
				cardView.DisableInteractivity();
			}

			Vector3 initialPosition = Card.transform.position;
			draggingOffset = initialPosition - Camera.ScreenToWorldPoint(Input.mousePosition);
			transform.position = initialPosition;

			foreach (CardView cardToDrag in allMovingCards)
			{
				cardToDrag.transform.SetParent(transform);
			}
		}

		public void Update()
		{
			if (Input.GetMouseButtonUp(InputTypes.LeftClick))
			{
				Drop();
			}
			else
			{
				Vector3 newPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
				newPosition += draggingOffset;
				newPosition.z = -5;
				transform.position = newPosition;
			}
		}

		public void Drop()
		{
			bool moved = false;
			Bounds bounds = Card.ColliderBounds;

			Collider2D overCollider = Physics2D.OverlapAreaAll(bounds.min, bounds.max)
				.OrderBy(c => ((Vector2)c.bounds.ClosestPoint(bounds.center) - (Vector2)bounds.center).magnitude).FirstOrDefault();

			if (overCollider != null)
			{
				CardPileView cardPileView = overCollider.GetComponentInParent<CardPileView>();

				if (cardPileView != null)
				{
					moved = TryPush(cardPileView, allMovingCards);
				}
			}

			foreach (CardView child in allMovingCards)
			{
				child.EnableInteractivity();
			}

			if (moved == false)
			{
				ResetCards();
			}

			Destroy(gameObject);
		}

		private bool TryPush(CardPileView toCardPile, IEnumerable<CardView> allMovingCards)
		{
			if (toCardPile.CanPush(Card))
			{
				CardPileView fromCardPile = Card.Parent;

				foreach (CardView cardToMove in allMovingCards.Reverse())
				{
					if (fromCardPile.Peek() != cardToMove)
					{
						throw new InvalidOperationException("Card to move is not its pile top card");
					}

					if (fromCardPile.CanPop() == false)
					{
						throw new InvalidOperationException("Card to move cannot be popped");
					}

					fromCardPile.Pop();
				}

				foreach (CardView cardToMove in allMovingCards)
				{
					if (toCardPile.CanPush(cardToMove) == false)
					{
						throw new InvalidOperationException("Card to move cannot be popped");
					}

					toCardPile.Push(cardToMove);
				}

				return true;
			}

			return false;
		}

		private void ResetCards()
		{
			foreach (CardView child in allMovingCards)
			{
				child.transform.SetParent(Card.Parent.transform, false);
			}

			Card.Parent.ResetDepth();
		}
	}
}
