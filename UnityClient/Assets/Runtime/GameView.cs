using BenjaminHamon.Solitaire.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	public class GameView : ViewElement
	{
		public GameView()
		{
			cardViewResolver = new CardViewResolver(this);
		}

		public Game Model
		{
			get { return (Game)ModelAsObject; }
			set { ModelAsObject = value; }
		}

		public Action Completed;

		public StockView Stock = null;
		public WasteView Waste = null;
		public FoundationView Foundation = null;
		public TableauView Tableau = null;

		private readonly CardViewResolver cardViewResolver;
		private readonly List<CardView> allCardViews = new List<CardView>();

		public IEnumerable<CardView> EnumerateAllCards()
		{
			return allCardViews.AsReadOnly();
		}

		[SerializeField]
		private GameObject victory = null;

		public void SetUp()
		{
			Stock.Model = Model.Stock;
			Waste.Model = Model.Waste;
			Foundation.Model = Model.Foundation;
			Tableau.Model = Model.Tableau;

			Stock.SetUp(cardViewResolver);
			Waste.SetUp(cardViewResolver);
			Foundation.SetUp(cardViewResolver);
			Tableau.SetUp(cardViewResolver);

			allCardViews.AddRange(Stock.EnumerateCards());
			allCardViews.AddRange(Waste.EnumerateCards());
			allCardViews.AddRange(Foundation.EnumerateCards());
			allCardViews.AddRange(Tableau.EnumerateCards());

			foreach (CardView cardView in allCardViews)
			{
				cardView.Foundation = Foundation;
			}
		}

		public void Start()
		{
			Model.Victory += HandleVictory;
		}

		public void OnEnable()
		{
			if (Model != null)
			{
				Model.Victory += HandleVictory;
			}
		}

		public void OnDisable()
		{
			if (Model != null)
			{
				Model.Victory -= HandleVictory;
			}
		}

		private void HandleVictory()
		{
			Debug.Log("[Game] Victory");
			StartCoroutine(ShowVictory());
		}

		private IEnumerator ShowVictory()
		{
			victory.SetActive(true);
			yield return new WaitForSeconds(2);
			Completed?.Invoke();
		}
	}
}
