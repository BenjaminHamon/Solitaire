using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using BenjaminHamon.Solitaire.UnityExtensions;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Controllers
{
	public class HumanController : MonoBehaviour
	{
		[SerializeField]
		private Camera Camera;
		[SerializeField]
		private GameObject DraggingHandlerPrefab;

		public GameView Game;

		private float lastMouseDownTime;
		private Vector3 lastMouseDownPosition;
		private Collider2D lastMouseDownCollider;
		private float lastMouseUpTime;
		private Vector3 lastMouseUpPosition;
		private Collider2D lastMouseUpCollider;

		private CardDraggingHandler draggingHandler;

		public void Update()
		{
			TryDrawOrResetStock();
			TryStartDraggingCard();
			TryRevealCard();
			TryPushCardToFoundation();

			if (Input.GetMouseButtonDown(InputTypes.LeftClick))
			{
				lastMouseDownTime = Time.time;
				lastMouseDownPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
				lastMouseDownCollider = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero).collider;
			}

			if (Input.GetMouseButtonUp(InputTypes.LeftClick))
			{
				lastMouseUpTime = Time.time;
				lastMouseUpPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
				lastMouseUpCollider = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero).collider;
			}
		}

		public bool TryDrawOrResetStock()
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
				if (Time.time - lastMouseDownTime > 0.05f) // Check for actual dragging and not click
				{
					RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

					if (hit.collider != null)
					{
						CardView card = hit.collider.GetComponent<CardView>();

						if ((card != null) && (card.IsVisible == true))
						{
							if (lastMouseDownCollider == hit.collider)
							{
								Vector3 mouseCurrentPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
								float dragDelta = ((Vector2)mouseCurrentPosition - (Vector2)lastMouseDownPosition).magnitude;

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
				if (Time.time - lastMouseUpTime < 0.5f) // Check for double click
				{
					RaycastHit2D hit = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

					if (hit.collider != null)
					{
						if (lastMouseUpCollider == hit.collider)
						{
							CardView card = hit.collider.GetComponent<CardView>();

							if (card != null)
							{
								Game.Foundation.TryPush(card);
							}
						}
						else
						{
							CardView card = hit.collider.GetComponent<CardView>();

							if (card != null)
							{
								card.TryReveal();
							}
						}
					}
				}
			}

			return false;
		}
	}
}
