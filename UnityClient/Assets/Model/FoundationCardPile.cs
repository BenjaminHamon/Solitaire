using System;
using System.Linq;

namespace BenjaminHamon.Solitaire.Model
{
	public class FoundationCardPile : CardPile
	{
		public FoundationCardPile(int cardMaxNumber)
		{
			this.cardMaxNumber = cardMaxNumber;
		}

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
			bool parentTypeIsAsExpected = (card.Parent is TableauCardPile) || (card.Parent is WasteCardPile);
			bool cardPositionInPileIsAsExpected = card.Parent.Peek() == card;

			if (parentTypeIsAsExpected && cardPositionInPileIsAsExpected)
			{
				Card topCard = cardCollection.FirstOrDefault();
				bool canPushAsFirstCard = (topCard == null) && (card.Number == 1);
				bool canPushAsNextCard = (topCard != null) && (topCard.Type == card.Type) && (topCard.Number == card.Number - 1);

				return canPushAsFirstCard || canPushAsNextCard;
			}

			return false;
		}

		public override void Push(Card card)
		{
			base.Push(card);

			if (IsComplete)
			{
				Completed?.Invoke();
			}
		}
	}
}
