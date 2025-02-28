using System;
using System.Linq;

namespace BenjaminHamon.Solitaire.UnityClient
{
	/// <summary>The foundations are the piles where the player must stack cards as same type sequences to achieve victory.</summary>
	public class FoundationCardPile : CardPile
	{
		public int CardMaxNumber;

		public event Action Completed;

		public bool IsComplete
		{
			get
			{
				Card topCard = Peek();
				return (topCard != null) && (topCard.Number == CardMaxNumber);
			}
		}

		public override bool CanPush(Card card)
		{
			Card topCard = Cards.FirstOrDefault();
			bool canPushAsFirstCard = (topCard == null) && (card.Number == 1);
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
	}
}
