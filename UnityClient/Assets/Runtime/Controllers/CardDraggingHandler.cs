using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
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
		public bool WasDroppedSuccessfully;

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
			Vector3 newPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
			newPosition += draggingOffset;
			newPosition.z = -5;
			transform.position = newPosition;
		}

		public CardPileView GetTarget()
		{
			Bounds bounds = Card.ColliderBounds;

			Collider2D overCollider = Physics2D.OverlapAreaAll(bounds.min, bounds.max)
				.OrderBy(c => ((Vector2)c.bounds.ClosestPoint(bounds.center) - (Vector2)bounds.center).magnitude).FirstOrDefault();

			if (overCollider != null)
			{
				return overCollider.GetComponentInParent<CardPileView>();
			}

			return null;
		}

		public void OnDestroy()
		{
			foreach (CardView child in allMovingCards)
			{
				child.EnableInteractivity();
			}

			if (WasDroppedSuccessfully == false)
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
