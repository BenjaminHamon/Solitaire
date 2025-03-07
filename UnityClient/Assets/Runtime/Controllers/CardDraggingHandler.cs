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
				// Disable interactivty for dragged cards to detect the card underneath on drop.
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

		private bool TryPush(CardPileView cardPileView, IEnumerable<CardView> allMovingCards)
		{
			if (cardPileView.CanPush(Card))
			{
				if (Card.Parent != null)
				{
					foreach (CardView cardToPop in allMovingCards.Reverse())
					{
						if (cardToPop.Parent.Peek() != cardToPop)
						{
							throw new InvalidOperationException("Parent last card is not as expected");
						}

						cardToPop.Parent.Pop();
					}
				}

				foreach (CardView cardToPush in allMovingCards)
				{
					cardPileView.Push(cardToPush);
				}

				return true;
			}

			return false;
		}

		private void ResetCards()
		{
			if (Card.Parent != null)
			{
				foreach (CardView child in allMovingCards)
				{
					child.transform.SetParent(Card.Parent.transform, false);
				}

				Card.Parent.ResetDepth();
			}
		}
	}
}
