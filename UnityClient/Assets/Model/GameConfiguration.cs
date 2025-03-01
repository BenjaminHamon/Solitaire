using System;
using System.Collections.Generic;

namespace BenjaminHamon.Solitaire.Model
{
	[Serializable]
	public class GameConfiguration
	{
		public List<CardType> CardTypes = new List<CardType>();
		public int CardMaxNumber;
		public List<int> Tableau = new List<int>();
	}
}
