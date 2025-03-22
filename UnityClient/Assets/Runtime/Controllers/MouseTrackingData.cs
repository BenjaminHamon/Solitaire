using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Controllers
{
    public class MouseTrackingData
	{
		public MouseTrackingData(float time, Vector2 position, Collider2D collider)
		{
			this.Time = time;
			this.Collider = collider;
			this.Position = position;
		}

		public float Time { get; }
		public Vector2 Position { get; }
		public Collider2D Collider { get; }
	}
}
