using System;

namespace BenjaminHamon.Solitaire.Model
{
	public class Application
	{
		public Game ActiveGame { get; private set; }
		public int? LastSeed { get; private set; }

		public Game NewGame(GameConfiguration configuration, int seed)
		{
			Random random = new Random(seed);
			Game game = new Game(configuration, random);

			this.ActiveGame = game;
			this.LastSeed = seed;

			return game;
		}

		public void EndGame()
		{
			this.ActiveGame = null;
		}
	}
}
