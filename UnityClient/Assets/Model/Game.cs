using System;
using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
    public class Game
    {
		public Game(GameConfiguration configuration, Random random)
		{
			this.configuration = configuration;
			this.random = random;
		}

		private readonly GameConfiguration configuration;
		private readonly Random random;

		public StockAndWaste StockAndWaste { get; private set; }
		public Foundation Foundation { get; private set; }
		public Tableau Tableau { get; private set; }

		public event Action Victory;

		public void Initialize()
		{
			StockAndWaste = null;
			Foundation = null;
			Tableau = null;

			List<Card> cardDeck = CreateDeck();
			SetUpCardPiles(cardDeck);

			Foundation.EnableEventListeners();
			Foundation.AllPilesCompleted += NotifyVictory;
		}

		public void EnableEventListeners()
		{
			Foundation.EnableEventListeners();
			Foundation.AllPilesCompleted += NotifyVictory;
		}

		public void DisableEventListeners()
		{
			Foundation.DisableEventListeners();
			Foundation.AllPilesCompleted -= NotifyVictory;
		}

		private List<Card> CreateDeck()
		{
			List<Card> cardDeck = new List<Card>();

			int numberInDeck = 1;

			foreach (CardType cardType in configuration.CardTypes)
			{
				for (int cardNumber = 1; cardNumber <= configuration.CardMaxNumber; cardNumber++)
				{
					Card newCard = new Card(numberInDeck, cardType, cardNumber);
					cardDeck.Add(newCard);
					numberInDeck += 1;
				}
			}

			cardDeck = cardDeck.OrderBy(c => random.Next()).ToList();

			return cardDeck;
		}

		private void SetUpCardPiles(IEnumerable<Card> cardDeck)
		{
			StockAndWaste = new StockAndWaste();
			Foundation = new Foundation();
			Tableau = new Tableau();

			Stack<Card> deckAsStack = new Stack<Card>(cardDeck);

			for (int foundationPileIndex = 0; foundationPileIndex < configuration.CardTypes.Count; foundationPileIndex++)
			{
				Foundation.AddCardPile(configuration.CardMaxNumber);
			}

			foreach (int pileCardSize in configuration.Tableau)
			{
				TableauCardPile tableauPile = Tableau.AddCardPile(configuration.CardMaxNumber);

				for (int pileCardIndex = 0; pileCardIndex < pileCardSize; pileCardIndex++)
				{
					tableauPile.Push(deckAsStack.Pop());
				}

				Card topCard = tableauPile.Peek();

				if (topCard != null)
				{
					topCard.Visible = true;
				}
			}

			foreach (Card card in deckAsStack)
			{
				StockAndWaste.PushToStock(card);
			}
		}

		private void NotifyVictory()
		{
			if (Foundation.AreAllPilesCompleted())
			{
				Victory?.Invoke();
			}
		}

		public bool IsVictoryAchieved()
		{
			return Foundation.AreAllPilesCompleted();
		}
	}
}
