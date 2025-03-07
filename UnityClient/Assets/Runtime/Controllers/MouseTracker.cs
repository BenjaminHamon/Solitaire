using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Controllers
{
	/// <summary>Track time, position and collider associated with mouse actions.</summary>
    public class MouseTracker
    {
		public Camera Camera { get; set; }

		public float LastMouseDownTime { get; private set; }
		public Vector3 LastMouseDownPosition { get; private set; }
		public Collider2D LastMouseDownCollider { get; private set; }
		public float LastMouseUpTime { get; private set; }
		public Vector3 LastMouseUpPosition { get; private set; }
		public Collider2D LastMouseUpCollider { get; private set; }

		public void SetMouseDownTracking()
		{
			LastMouseDownTime = Time.time;
			LastMouseDownPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
			LastMouseDownCollider = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero).collider;
		}

		public void SetMouseUpTracking()
		{
			LastMouseUpTime = Time.time;
			LastMouseUpPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
			LastMouseUpCollider = Physics2D.Raycast(Camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero).collider;
		}

		public void UnsetMouseDownTracking()
		{
			LastMouseDownTime = 0;
			LastMouseDownPosition = Vector3.zero;
			LastMouseDownCollider = null;
		}

		public void UnsetMouseUpTracking()
		{
			LastMouseUpTime = 0;
			LastMouseUpPosition = Vector3.zero;
			LastMouseUpCollider = null;
		}
	}
}
