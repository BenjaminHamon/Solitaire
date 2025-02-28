using BenjaminHamon.Solitaire.UnityClient.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient
{
	public class Card : MonoBehaviour
	{
		[SerializeField]
		private new SpriteRenderer renderer = null;
		[SerializeField]
		private new Collider2D collider = null;
		[SerializeField]
		private Sprite frontSprite = null;
		[SerializeField]
		private Sprite backSprite = null;

		public Collider2D Collider { get { return collider; } }

		public Game Game;
		public CardPile Parent;

		private CardType type;
		[ExposeProperty]
		public CardType Type
		{
			get { return type; }
			set
			{
				if (type == value)
					return;

				type = value;

				UpdateSprite();
			}
		}

		private int number;
		[ExposeProperty]
		public int Number
		{
			get { return number; }
			set
			{
				if (number == value)
					return;

				number = value;

				UpdateSprite();
			}
		}

		private bool visible;
		[ExposeProperty]
		public bool Visible
		{
			get { return visible; }
			set
			{
				if (visible == value)
					return;

				visible = value;

				renderer.sprite = visible ? frontSprite : backSprite;
			}
		}

		private Transform draggingHandler;
		private Vector3 draggingStartingPoint;
		private Vector3 draggingOffset;

		private float lastClick;

		public void OnEnable()
		{
			UpdateSprite();
		}

		private void UpdateSprite()
		{
			if (isActiveAndEnabled || (UnityEngine.Application.isPlaying == false))
			{
				frontSprite = Application.AssetLoader.LoadOrDefaultByPath<Sprite>(AssetBundleNames.Cards, "Sprites/Cards/Card" + type + number + ".png");
				renderer.sprite = visible ? frontSprite : backSprite;
			}
		}

		public void OnMouseDown()
		{
			draggingStartingPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		public void OnMouseDrag()
		{
			if (Visible == false)
				return;

			Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (draggingHandler == null)
			{
				float dragDelta = ((Vector2)newPosition - (Vector2)draggingStartingPoint).magnitude;
				if (dragDelta > 0.1f)
					StartDragging();
			}

			if (draggingHandler != null)
			{
				newPosition += draggingOffset;
				newPosition.z = -5;
				draggingHandler.position = newPosition;
			}
		}

		public void OnMouseUp()
		{
			if (Time.time - lastClick > 0.5f)
			{
				lastClick = Time.time;
			}
			else
			{
				TryPushToFoundation();
				lastClick = 0;
			}

			TryRevealInTableau();

			if (draggingHandler != null)
			{
				Drop();
			}
		}

		private void TryPushToFoundation()
		{
			if (Parent != null)
			{
				bool parentTypeIsAsExpected = (Parent is TableauCardPile) || (Parent is WasteCardPile);
				bool cardPositionInPileIsAsExpected = Parent.Peek() == this;

				if (parentTypeIsAsExpected && cardPositionInPileIsAsExpected)
				{
					foreach (FoundationCardPile foundationCardPile in Game.FoundationCardPiles)
					{
						if (foundationCardPile.CanPush(this))
						{
							Parent.Pop();
							foundationCardPile.Push(this);
							break;
						}
					}
				}
			}
		}

		private void TryRevealInTableau()
		{
			if (Visible == true)
				return;

			if (Parent != null)
			{
				bool parentTypeIsAsExpected = Parent is TableauCardPile;
				bool cardPositionInPileIsAsExpected = Parent.Peek() == this;

				if (parentTypeIsAsExpected && cardPositionInPileIsAsExpected)
				{
					Visible = true;
				}
			}
		}

		private void StartDragging()
		{
			// Debug.Log("[Card] StartDragging");

			Vector3 initialPosition = transform.position;
			draggingOffset = initialPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);

			draggingHandler = new GameObject().transform;
			draggingHandler.name = "DraggingHandler";
			draggingHandler.position = initialPosition;

			if (Parent is TableauCardPile parentAsTableauCardPile)
			{
				foreach (Card card in parentAsTableauCardPile.EnumerateCardsFrom(this))
				{
					card.transform.SetParent(draggingHandler);
				}
			}
			else
			{
				transform.SetParent(draggingHandler);
			}
		}

		private void Drop()
		{
			// Debug.Log("[Card] Drop");

			List<Card> allMovingCards;

			if (Parent is TableauCardPile parentAsTableauCardPile)
			{
				allMovingCards = parentAsTableauCardPile.EnumerateCardsFrom(this).ToList();
			}
			else
			{
				allMovingCards = new List<Card>() { this };
			}

			bool moved = false;
			Bounds bounds = collider.bounds;
			// Debug.DrawLine(bounds.min, bounds.max, Color.red, 3);

			// Disable dragged card colliders to detect the collider under them
			foreach (Card card in allMovingCards)
			{
				card.collider.enabled = false;
			}

			Collider2D overCollider = Physics2D.OverlapAreaAll(bounds.min, bounds.max)
				.OrderBy(c => ((Vector2)c.bounds.ClosestPoint(bounds.center) - (Vector2)bounds.center).magnitude).FirstOrDefault();

			if (overCollider != null)
			{
				// Debug.Log("[Card] Drop on " + overCollider.name, overCollider);
				CardPile cardPile = overCollider.GetComponentInParent<CardPile>();

				if ((cardPile != null) && cardPile.CanPush(this))
				{
					if (Parent != null)
					{
						foreach (Card cardToPop in ((IEnumerable<Card>)allMovingCards).Reverse())
						{
							if (cardToPop.Parent.Peek() != cardToPop)
							{
								throw new InvalidOperationException("Parent last card is not as expected");
							}

							cardToPop.Parent.Pop();
						}
					}

					foreach (Card cardToPush in allMovingCards)
					{
						cardPile.Push(cardToPush);
					}

					moved = true;
				}
			}

			foreach (Card child in allMovingCards)
			{
				child.collider.enabled = true;
			}

			if (moved == false)
			{
				foreach (Card child in allMovingCards)
				{
					child.transform.SetParent(Parent.transform, false);
				}

				Parent.ResetDepth();
			}

			Destroy(draggingHandler.gameObject);
		}

		public override string ToString()
		{
			return String.Format("Card {0} {1}", type, number);
		}
	}
}
