using BenjaminHamon.Solitaire.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
    public class CardViewResolver
    {
		public CardViewResolver(GameView gameView)
		{
			this.gameView = gameView;
		}

		private readonly GameView gameView;

		public CardView GetCardView(Card card)
		{
			return gameView.EnumerateAllCards().Single(x => x.Model == card);
		}
	}
}
