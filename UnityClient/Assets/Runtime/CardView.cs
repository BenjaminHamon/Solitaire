using BenjaminHamon.Solitaire.Model;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	public class CardView : ViewElement
	{
		public Card Model
		{
			get { return (Card)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		[SerializeField]
		private new SpriteRenderer renderer = null;
		[SerializeField]
		private new Collider2D collider = null;
		[SerializeField]
		private Sprite frontSprite = null;
		[SerializeField]
		private Sprite backSprite = null;

		public FoundationView Foundation;
		public CardPileView Parent;

		private Transform draggingHandler;
		private Vector3 draggingStartingPoint;
		private Vector3 draggingOffset;

		private float lastClick;

		public void Start()
		{
			string frontAssetPath = "Sprites/Cards/Card" + Model.Type + Model.Number + ".png";
			frontSprite = ApplicationStatic.AssetLoader.LoadOrDefaultByPath<Sprite>(AssetBundleNames.Cards, frontAssetPath);

			string backAssetPath = "Sprites/CardBacks/CardBackBlue1.png";
			backSprite = ApplicationStatic.AssetLoader.LoadOrDefaultByPath<Sprite>(AssetBundleNames.Cards, backAssetPath);

			UpdateVisiblity();

			Model.VisiblityChanged += UpdateVisiblity;
		}

		public void OnEnable()
		{
			if (Model != null)
			{
				Model.VisiblityChanged += UpdateVisiblity;
			}
		}

		public void OnDisable()
		{
			if (Model != null)
			{
				Model.VisiblityChanged -= UpdateVisiblity;
			}
		}

		private void UpdateVisiblity()
		{
			renderer.sprite = Model.Visible ? frontSprite : backSprite;
		}

		public void EnableInteractivity()
		{
			collider.enabled = true;
		}

		public void DisableInteractivity()
		{
			collider.enabled = false;
		}

		public void OnMouseDown()
		{
			draggingStartingPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		public void OnMouseDrag()
		{
			if (Model.Visible == false)
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

			Model.TryReveal();

			if (draggingHandler != null)
			{
				Drop();
			}
		}

		private void TryPushToFoundation()
		{
			if (Parent != null)
			{
				bool parentTypeIsAsExpected = (Parent is TableauCardPileView) || (Parent is WasteCardPileView);
				bool cardPositionInPileIsAsExpected = Parent.Peek() == this;

				if (parentTypeIsAsExpected && cardPositionInPileIsAsExpected)
				{
					foreach (FoundationCardPileView foundationCardPile in Foundation.EnumeratePiles())
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

		private void StartDragging()
		{
			// Debug.Log("[Card] StartDragging");

			Vector3 initialPosition = transform.position;
			draggingOffset = initialPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);

			draggingHandler = new GameObject().transform;
			draggingHandler.name = "DraggingHandler";
			draggingHandler.position = initialPosition;

			if (Parent is TableauCardPileView parentAsTableauCardPile)
			{
				foreach (CardView card in parentAsTableauCardPile.EnumerateCardsFrom(this))
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

			List<CardView> allMovingCards;

			if (Parent is TableauCardPileView parentAsTableauCardPile)
			{
				allMovingCards = parentAsTableauCardPile.EnumerateCardsFrom(this).ToList();
			}
			else
			{
				allMovingCards = new List<CardView>() { this };
			}

			bool moved = false;
			Bounds bounds = collider.bounds;
			// Debug.DrawLine(bounds.min, bounds.max, Color.red, 3);

			// Disable dragged card colliders to detect the collider under them
			foreach (CardView card in allMovingCards)
			{
				card.collider.enabled = false;
			}

			Collider2D overCollider = Physics2D.OverlapAreaAll(bounds.min, bounds.max)
				.OrderBy(c => ((Vector2)c.bounds.ClosestPoint(bounds.center) - (Vector2)bounds.center).magnitude).FirstOrDefault();

			if (overCollider != null)
			{
				// Debug.Log("[Card] Drop on " + overCollider.name, overCollider);
				CardPileView cardPileView = overCollider.GetComponentInParent<CardPileView>();

				if ((cardPileView != null) && cardPileView.CanPush(this))
				{
					if (Parent != null)
					{
						foreach (CardView cardToPop in ((IEnumerable<CardView>)allMovingCards).Reverse())
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

					moved = true;
				}
			}

			foreach (CardView child in allMovingCards)
			{
				child.collider.enabled = true;
			}

			if ((moved == false) && (Parent != null))
			{
				foreach (CardView child in allMovingCards)
				{
					child.transform.SetParent(Parent.transform, false);
				}

				Parent.ResetDepth();
			}

			Destroy(draggingHandler.gameObject);
		}

		public override string ToString()
		{
			return String.Format("CardView {0} ({1} {2})", Model.NumberInDeck, Model.Type, Model.Number);
		}
	}
}
