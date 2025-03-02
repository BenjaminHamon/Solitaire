using System;

namespace BenjaminHamon.Solitaire.Model
{
	public class Card
	{
		public Card(int numberInDeck, CardType type, int number)
		{
			this.NumberInDeck = numberInDeck;
			this.Type = type;
			this.Number = number;
		}

		public int NumberInDeck { get; }
		public CardType Type { get; }
		public int Number { get; }

		public CardPile Parent { get; set; }

		public Action VisiblityChanged;

		private bool visibleField;
		public bool Visible
		{
			get
			{
				return visibleField;
			}
			set
			{
				visibleField = value;
				VisiblityChanged?.Invoke();
			}
		}

		public void TryReveal()
		{
			if (Visible == true)
				return;

			if (Parent != null)
			{
				bool parentTypeIsAsExpected = Parent is TableauCardPile;
				bool cardPositionInPileIsAsExpected = Parent.Peek() == this;

				if (parentTypeIsAsExpected && cardPositionInPileIsAsExpected)
				{
					Visible = true;
				}
			}
		}

		public override string ToString()
		{
			return String.Format("Card {0} ({1} {2})", NumberInDeck, Type, Number);
		}
	}
}
