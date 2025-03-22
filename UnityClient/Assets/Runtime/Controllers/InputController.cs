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

		public GameView Game;

		private readonly MouseTracker MouseTracker = new MouseTracker();
		private CardDraggingHandler DraggingHandler;

		private void Start()
		{
			MouseTracker.Camera = Camera;
		}

		public void Update()
		{
			MouseTracker.UpdateBefore();

			TryDrawOrResetStock();
			TryRevealCard();
			TryPushCardToFoundation();
			TryStartDraggingCard();

			MouseTracker.UpdateAfter();
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

				if (card != null)
				{
					return Game.Foundation.TryPush(card);
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

				if ((card != null) && (card.IsVisible == true))
				{
					GameObject draggingHandlerGameObject = Instantiate(DraggingHandlerPrefab, transform);
					draggingHandlerGameObject.name = "DraggingHandler";

					DraggingHandler = draggingHandlerGameObject.GetComponent<CardDraggingHandler>();
					DraggingHandler.Camera = Camera;
					DraggingHandler.Card = card;

					return true;
				}
			}

			return false;
		}
	}
}
