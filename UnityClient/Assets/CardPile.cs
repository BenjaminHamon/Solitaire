using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BenjaminHamon.Solitaire.UnityClient
{
	public class CardPile : MonoBehaviour
	{
		protected Stack<Card> Cards = new Stack<Card>();

		public Card Peek()
		{
			return Cards.FirstOrDefault();
		}

		public virtual bool CanPush(Card card)
		{
			return false;
		}

		public virtual void Push(Card card)
		{
			Card topCard = Cards.FirstOrDefault();
			Vector3 cardPosition = Vector3.zero;

			if (topCard != null)
			{
				cardPosition = topCard.transform.localPosition;
			}

			cardPosition.z -= 0.1f;
			card.transform.localPosition = cardPosition;

			card.transform.SetParent(transform, false);
			card.Parent = this;
			Cards.Push(card);
		}

		public Card Pop()
		{
			Card card = Cards.Pop();
			card.Parent = null;
			return card;
		}

		public void ResetDepth()
		{
			float z = -0.1f * Cards.Count;

			foreach (Card card in Cards)
			{
				Vector3 cardPosition = card.transform.localPosition;
				cardPosition.z = z;
				card.transform.localPosition = cardPosition;
				z += 0.1f;
			}
		}
	}
}
