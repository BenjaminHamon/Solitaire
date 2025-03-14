// cspell:words raycast

using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using BenjaminHamon.Solitaire.UnityExtensions.Runtime;
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

		private readonly MouseTracker mouseTracker = new MouseTracker();
		private CardDraggingHandler draggingHandler;

		private void Start()
		{
			mouseTracker.Camera = Camera;
		}

		public void Update()
		{
			bool performedAction = false;

			if ((mouseTracker.LastMouseDownTime != 0) && (Time.time > mouseTracker.LastMouseDownTime + 1))
			{
				mouseTracker.UnsetMouseDownTracking();
			}

			if ((mouseTracker.LastMouseUpTime != 0) && (Time.time > mouseTracker.LastMouseUpTime + 1))
			{
				mouseTracker.UnsetMouseUpTracking();
			}

			if (TryDrawOrResetStock()) { performedAction = true; }
			if (TryStartDraggingCard()) { performedAction = true; }
			if (TryRevealCard()) { performedAction = true; }
			if (TryPushCardToFoundation()) { performedAction = true; }

			if ((performedAction == false) && Input.GetMouseButtonDown(InputTypes.LeftClick))
			{
				mouseTracker.SetMouseDownTracking();
			}

			if ((performedAction == false) && Input.GetMouseButtonUp(InputTypes.LeftClick))
			{
				mouseTracker.SetMouseUpTracking();
			}

			if (performedAction)
			{
				// Reset mouse tracking between two user actions
				// to avoid things like PushCardToFoundation acting on a double click from a previous click acted on by RevealCard.

				mouseTracker.UnsetMouseDownTracking();
				mouseTracker.UnsetMouseUpTracking();
			}
		}

		private bool TryDrawOrResetStock()
		{
			if (Input.GetMouseButtonUp(InputTypes.LeftClick))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null)
				{
					StockAndWasteView stockAndWaste = hit.collider.GetComponent<StockAndWasteView>();

					if (stockAndWaste != null)
					{
						stockAndWaste.TryDrawOrReset();

						return true;
					}
				}
			}

			return false;

		}

		private bool TryStartDraggingCard()
		{
			if (draggingHandler != null)
				return false;

			if (Input.GetMouseButton(InputTypes.LeftClick))
			{
				if (Time.time > mouseTracker.LastMouseDownTime) // Check for actual dragging and not click
				{
					RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

					if (hit.collider != null)
					{
						CardView card = hit.collider.GetComponent<CardView>();

						if ((card != null) && (card.IsVisible == true))
						{
							if (mouseTracker.LastMouseDownCollider == hit.collider)
							{
								Vector3 mouseCurrentPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
								float dragDelta = ((Vector2)mouseCurrentPosition - (Vector2)mouseTracker.LastMouseDownPosition).magnitude;

								if (dragDelta > 0.1f) // Check for actual dragging and not long click
								{
									GameObject draggingHandlerGameObject = Instantiate(DraggingHandlerPrefab);
									draggingHandlerGameObject.name = "DraggingHandler";

									draggingHandler = draggingHandlerGameObject.GetComponent<CardDraggingHandler>();
									draggingHandler.Camera = Camera;
									draggingHandler.Card = card;

									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		private bool TryRevealCard()
		{
			if (Input.GetMouseButtonUp(InputTypes.LeftClick))
			{
				RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null)
				{
					CardView card = hit.collider.GetComponent<CardView>();

					if (card != null)
					{
						return card.TryReveal();
					}
				}
			}

			return false;
		}

		private bool TryPushCardToFoundation()
		{
			if (Input.GetMouseButtonUp(InputTypes.LeftClick))
			{
				if (Time.time - mouseTracker.LastMouseUpTime < 0.5f) // Check for double click
				{
					RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

					if (hit.collider != null)
					{
						if (mouseTracker.LastMouseUpCollider == hit.collider)
						{
							CardView card = hit.collider.GetComponent<CardView>();

							if (card != null)
							{
								return Game.Foundation.TryPush(card);
							}
						}
					}
				}
			}

			return false;
		}
	}
}
