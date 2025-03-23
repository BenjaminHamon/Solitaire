// cspell:words raycast

using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Controllers
{
	/// <summary>Triggers game actions as commanded by input from an actual user.</summary>
	public class InputController : MonoBehaviour
	{
		[SerializeField]
		private Camera Camera;
		[SerializeField]
		private GameObject DraggingHandlerPrefab;
		[SerializeField]
		private GameObject CardPileSelectionPrefab;

		public GameView Game;

		private readonly MouseTracker MouseTracker = new MouseTracker();
		private CardDraggingHandler DraggingHandler;
		private CardPileView CardPileSelection;
		private GameObject CardPileSelectionGameObject;

		private void Start()
		{
			MouseTracker.Camera = Camera;
		}

		public void Update()
		{
			MouseTracker.UpdateBefore();

			bool performedGameChange
				= TryDrawOrResetStock()
				|| TryRevealCard()
				|| TryPushCardToFoundation()
				|| TryMoveSelectedCardPile();

			_ = performedGameChange
				|| TryStartDraggingCard()
				|| TrySelectCardFile();

			MouseTracker.UpdateAfter();

			if (performedGameChange)
			{
				MouseTracker.Clear();
			}
		}

		private bool TryDrawOrResetStock()
		{
			if (MouseTracker.IsClick())
			{
				StockAndWasteView stockAndWaste = MouseTracker.CurrentMouseUpEvent.Collider.GetComponent<StockAndWasteView>();

				if (stockAndWaste != null)
				{
					stockAndWaste.TryDrawOrReset();

					return true;
				}
			}

			return false;
		}

		private bool TryRevealCard()
		{
			if (MouseTracker.IsClick())
			{
				CardView card = MouseTracker.CurrentMouseUpEvent.Collider.GetComponent<CardView>();

				if (card != null)
				{
					return card.TryReveal();
				}
			}

			return false;
		}

		private bool TryPushCardToFoundation()
		{
			if (MouseTracker.IsDoubleClick())
			{
				CardView card = MouseTracker.CurrentMouseUpEvent.Collider.GetComponent<CardView>();

				if ((card != null) && (card.Parent.Peek() == card))
				{
					bool result = Game.Foundation.TryPush(card);

					if (result)
					{
						ClearSelection();
					}

					return result;
				}
			}

			return false;
		}

		private bool TryMoveSelectedCardPile()
		{
			if (CardPileSelection == null)
				return false;

			if (MouseTracker.IsClick())
			{
				CardPileView targetCardPile = MouseTracker.CurrentMouseUpEvent.Collider.GetComponentInParent<CardPileView>();

				if (targetCardPile != null)
				{
					if (CardPileSelection != targetCardPile)
					{
						bool result = Game.TryMoveCardPile(CardPileSelection, targetCardPile);

						if (result)
						{
							ClearSelection();
						}

						return result;
					}
				}
			}

			return false;
		}

		private bool TryStartDraggingCard()
		{
			if (DraggingHandler != null)
				return false;

			if (MouseTracker.IsDragging())
			{
				CardView card = MouseTracker.CurrentMouseDown.Collider.GetComponent<CardView>();

				if ((card != null) && card.IsVisible)
				{
					GameObject draggingHandlerGameObject = Instantiate(DraggingHandlerPrefab, transform);
					draggingHandlerGameObject.name = "DraggingHandler";

					DraggingHandler = draggingHandlerGameObject.GetComponent<CardDraggingHandler>();
					DraggingHandler.Camera = Camera;
					DraggingHandler.Card = card;

					ClearSelection();

					return true;
				}
			}

			return false;
		}

		private bool TrySelectCardFile()
		{
			if (MouseTracker.CurrentMouseUpEvent != null)
			{
				if (MouseTracker.CurrentMouseUpEvent.Collider != null)
				{
					CardView card = MouseTracker.CurrentMouseUpEvent.Collider.GetComponent<CardView>();

					if (card != null)
					{
						if (card.Parent is StockCardPileView)
							return false;

						if (CardPileSelection != card.Parent)
						{
							ClearSelection();

							GameObject newSelectionObject = Instantiate(CardPileSelectionPrefab, transform);
							newSelectionObject.name = "Selection";
							newSelectionObject.transform.position = card.Parent.transform.position + new Vector3(0, 0, 1);

							int cardCount = card.Parent.CardCount;
							Rect cardSpriteRectangle = card.GetComponent<SpriteRenderer>().sprite.rect;
							SpriteRenderer selectionSprite = newSelectionObject.GetComponent<SpriteRenderer>();

							// (width + border, height + border) / scaling factor between rect and size
							selectionSprite.size = new Vector2(cardSpriteRectangle.width + 20, cardSpriteRectangle.height + 20) / 100;

							if (card.Parent is TableauCardPileView)
							{
								// previous result + card offset * card offset height
								selectionSprite.size += new Vector2(0, (cardCount - 1) * 0.5f);
							}

							CardPileSelection = card.Parent;
							CardPileSelectionGameObject = newSelectionObject;

							return true;
						}
					}
				}

				ClearSelection();
			}

			return false;
		}

		private void ClearSelection()
		{
			if (CardPileSelection != null)
			{
				Destroy(CardPileSelectionGameObject);
			}

			CardPileSelection = null;
			CardPileSelectionGameObject = null;
		}
	}
}
