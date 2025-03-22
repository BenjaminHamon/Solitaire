// cspell:words raycast

using BenjaminHamon.Solitaire.UnityExtensions.Runtime;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Controllers
{
	/// <summary>Track time, position and collider associated with mouse actions.</summary>
    public class MouseTracker
    {
		public Camera Camera { get; set; }

		public MouseTrackingData LastMouseDownEvent { get; private set; }
		public MouseTrackingData LastMouseDown { get; private set; }
		public MouseTrackingData LastMouseUpEvent { get; private set; }
		public MouseTrackingData LastMouseUp { get; private set; }
		public MouseTrackingData CurrentMouseDownEvent { get; private set; }
		public MouseTrackingData CurrentMouseDown { get; private set; }
		public MouseTrackingData CurrentMouseUpEvent { get; private set; }
		public MouseTrackingData CurrentMouseUp { get; private set; }

		public void UpdateBefore()
		{
			CurrentMouseDownEvent = null;
			CurrentMouseDown = null;
			CurrentMouseUpEvent = null;
			CurrentMouseUp = null;

			MouseTrackingData localData = GetMouseTrackingData();

			if (Input.GetMouseButtonDown(InputTypes.LeftClick))
			{
				CurrentMouseDownEvent = localData;
			}

			if (Input.GetMouseButtonDown(InputTypes.LeftClick))
			{
				CurrentMouseUpEvent = localData;
			}

			if (Input.GetMouseButton(InputTypes.LeftClick))
			{
				CurrentMouseDown = localData;
			}
			else
			{
				CurrentMouseUp = localData;
			}
		}

		public void UpdateAfter()
		{
			if (CurrentMouseDownEvent != null)
			{
				LastMouseDownEvent = CurrentMouseUpEvent;
			}

			if (CurrentMouseUpEvent != null)
			{
				LastMouseUpEvent = CurrentMouseUpEvent;
			}

			LastMouseDown = CurrentMouseDown;
			LastMouseUp = CurrentMouseUp;

			if ((LastMouseDownEvent != null) && (Time.time > LastMouseDownEvent.Time + 1))
			{
				LastMouseDownEvent = null;
			}

			if ((LastMouseUpEvent != null) && (Time.time > LastMouseUpEvent.Time + 1))
			{
				LastMouseUpEvent = null;
			}
		}

		public bool IsClick()
		{
			return (CurrentMouseUpEvent != null) && (CurrentMouseUpEvent.Collider != null);
		}

		public bool IsDoubleClick()
		{
			bool clickedTwice = (CurrentMouseUpEvent != null) && (LastMouseUpEvent != null);

			if (clickedTwice == false)
				return false;
			
			bool shortTimeElapsed = (CurrentMouseUpEvent.Time - LastMouseUpEvent.Time < 0.5f);
			bool sameCollider = (CurrentMouseUpEvent.Collider != null) && (CurrentMouseUpEvent.Collider == LastMouseUpEvent.Collider);

			return shortTimeElapsed && sameCollider;
		}

		public bool IsDragging()
		{
			if (CurrentMouseDown == null)
				return false;

			if (LastMouseDownEvent == null)
				return false;

			bool notClick = CurrentMouseDown.Time > LastMouseDownEvent.Time + 0.05f;
			bool moved = (CurrentMouseDown.Position - LastMouseDownEvent.Position).magnitude > 0.1f;
			bool sameCollider = (CurrentMouseDown.Collider != null) && (CurrentMouseDown.Collider == LastMouseDownEvent.Collider);

			return notClick && moved && sameCollider;
		}

		private MouseTrackingData GetMouseTrackingData()
		{
			return new MouseTrackingData(Time.time,
				Camera.ScreenToWorldPoint(Input.mousePosition),
				Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero).collider);
		}
	}
}
