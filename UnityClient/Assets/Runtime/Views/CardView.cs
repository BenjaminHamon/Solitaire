using BenjaminHamon.Solitaire.Model;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using System;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime.Views
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

		public CardPileView Parent;
		public bool IsVisible { get { return Model.Visible; } }
		public Bounds ColliderBounds { get { return collider.bounds; } }

		public void Start()
		{
			string frontAssetPath = "Sprites/Cards/Card" + Model.Type + Model.Number + ".png";
			frontSprite = ApplicationStatic.Application.AssetLoader.LoadOrDefaultByPath<Sprite>(AssetBundleNames.Cards, frontAssetPath);

			string backAssetPath = "Sprites/CardBacks/CardBackBlue1.png";
			backSprite = ApplicationStatic.Application.AssetLoader.LoadOrDefaultByPath<Sprite>(AssetBundleNames.Cards, backAssetPath);

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

		public bool TryReveal()
		{
			return Model.TryReveal();
		}

		public override string ToString()
		{
			return String.Format("CardView {0} ({1} {2})", Model.NumberInDeck, Model.Type, Model.Number);
		}
	}
}
