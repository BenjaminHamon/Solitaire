using BenjaminHamon.Solitaire.Model;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using System.Linq;

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
