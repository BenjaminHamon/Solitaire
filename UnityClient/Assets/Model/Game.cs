using System;
using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	// Some notes about the implementation of the game, its card piles and its cards.
	//
	// The responsibility of moving cards, meaning popping and pushing,
	// is a responsibility of the main game components (foundation, stock and waste, tableau), not the card piles.
	// This operation implies a pop then a push, hence the card pile's pop method should expect the card's parent to be null.

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
				for (int cardNumber = configuration.CardMinNumber; cardNumber <= configuration.CardMaxNumber; cardNumber++)
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
				Foundation.AddCardPile(configuration.CardMinNumber, configuration.CardMaxNumber);
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
				StockAndWaste.StockCardPile.Push(card);
			}
		}

		public bool TryMoveCardsFromPile(CardPile fromCardPile, CardPile toCardPile)
		{
			if ((fromCardPile is StockCardPile) || (toCardPile is StockCardPile))
				return false;

			if (toCardPile is WasteCardPile)
				return false;

			if (fromCardPile is TableauCardPile fromCardPileAsTableau)
			{
				foreach (Card card in fromCardPile.EnumerateCards())
				{
					if (TryMoveCard(card, toCardPile))
					{
						return true;
					}
				}
			}
			else
			{
				return TryMoveCard(fromCardPile.Peek(), toCardPile);
			}

			return false;
		}

		public bool TryMoveCard(Card fromCard, CardPile toCardPile)
		{
			if ((fromCard.Parent is StockCardPile) || (toCardPile is StockCardPile))
				return false;

			if (toCardPile is WasteCardPile)
				return false;

			List<Card> allCardsToMove = null;

			if (toCardPile.CanPush(fromCard) == false)
				return false;

			if (fromCard.Parent is TableauCardPile fromCardPileAsTableau)
			{
				allCardsToMove = fromCardPileAsTableau.EnumerateCardsFrom(fromCard).ToList();
			}
			else
			{
				if (fromCard.Parent.CanPop())
				{
					allCardsToMove = new List<Card>() { fromCard };
				}
			}

			if (allCardsToMove != null)
			{
				if ((toCardPile is FoundationCardPile) && (allCardsToMove.Count > 1))
					return false;

				foreach (Card cardToMove in allCardsToMove.Reverse<Card>())
				{
					if (cardToMove.Parent.CanPop() == false)
					{
						throw new InvalidOperationException("Card to move cannot be popped");
					}

					Card poppedCard = fromCard.Parent.Pop();

					if (poppedCard != cardToMove)
					{
						throw new InvalidOperationException("Popped card is not as expected");
					}
				}

				foreach (Card cardToMove in allCardsToMove)
				{
					if (toCardPile.CanPush(cardToMove) == false)
					{
						throw new InvalidOperationException("Card to move cannot be pushed");
					}

					toCardPile.Push(cardToMove);
				}

				return true;
			}

			return false;
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
