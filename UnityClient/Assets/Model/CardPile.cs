using System;
using System.Collections.Generic;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	/// <summary>Base class for all card piles.</summary>
	public abstract class CardPile
	{
		protected Stack<Card> cardCollection = new Stack<Card>();

		public event Action<Card> CardPushed;
		public event Action<Card> CardPopped;

		public IEnumerable<Card> EnumerateCards()
		{
			return cardCollection.ToList().AsReadOnly();
		}

		public Card Peek()
		{
			return cardCollection.FirstOrDefault();
		}

		public abstract bool CanPush(Card card);

		public virtual void Push(Card card)
		{
			Card topCard = cardCollection.FirstOrDefault();
			card.Parent = this;
			cardCollection.Push(card);

			CardPushed?.Invoke(card);
		}

		public abstract bool CanPop();

		public Card Pop()
		{
			Card card = cardCollection.Pop();
			card.Parent = null;

			CardPopped?.Invoke(card);

			return card;
		}
	}
}
