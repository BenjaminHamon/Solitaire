using BenjaminHamon.Solitaire.Model;
using System;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	/// <summary>The foundations are the piles where the player must stack cards as same type sequences to achieve victory.</summary>
	public class FoundationCardPileView : CardPileView
	{
		public new FoundationCardPile Model
		{
			get { return (FoundationCardPile)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		public event Action Completed;

		public override void Start()
		{
			base.Start();

			Model.Completed += HandleCompleted;
		}

		public override void OnEnable()
		{
			base.OnEnable();

			if (Model != null)
			{
				Model.Completed += HandleCompleted; 
			}
		}

		public override void OnDisable()
		{
			Model.Completed -= HandleCompleted;

			if (Model != null)
			{
				base.OnDisable(); 
			}
		}

		public bool IsComplete
		{
			get
			{
				return Model.IsComplete;
			}
		}

		private void HandleCompleted()
		{
			Completed?.Invoke();
		}
	}
}
