using System;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	public class FoundationCardPile : CardPile
	{
		public FoundationCardPile(int cardMinNumber, int cardMaxNumber)
		{
			this.cardMinNumber = cardMinNumber;
			this.cardMaxNumber = cardMaxNumber;
		}

		private readonly int cardMinNumber;
		private readonly int cardMaxNumber;

		public event Action Completed;

		public bool IsComplete
		{
			get
			{
				Card topCard = Peek();
				return (topCard != null) && (topCard.Number == cardMaxNumber);
			}
		}

		public override bool CanPush(Card card)
		{
			if (card.Visible == false)
				return false;

			Card topCard = cardCollection.FirstOrDefault();
			bool canPushAsFirstCard = (topCard == null) && (card.Number == cardMinNumber);
			bool canPushAsNextCard = (topCard != null) && (topCard.Type == card.Type) && (topCard.Number == card.Number - 1);

			return canPushAsFirstCard || canPushAsNextCard;
		}

		public override void Push(Card card)
		{
			base.Push(card);

			if (IsComplete)
			{
				Completed?.Invoke();
			}
		}

		public override bool CanPop()
		{
			return cardCollection.Any();
		}
	}
}
