using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	public class GameMenu : MonoBehaviour
	{
		[SerializeField]
		private InputField seedInput = null;
		[SerializeField]
		private Button lastSeedButton = null;
		[SerializeField]
		private bool randomSeedOnStart = true;

		private int seed
		{
			get { return Int32.Parse(seedInput.text); }
			set { seedInput.text = value.ToString(); }
		}

		public void Start()
		{
			if (randomSeedOnStart)
			{
				SetRandomSeed();
			}

			if (ApplicationStatic.GameSeed != null)
			{
				lastSeedButton.interactable = true;
			}
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
				Exit();
		}

		public void StartNewGame()
		{
			ApplicationStatic.GameSeed = seed;
			SceneManager.LoadScene("GameScene");
		}

		public void SetRandomSeed()
		{
			System.Random random = new System.Random();
			seed = random.Next();
		}

		public void SetLastSeed()
		{
			seed = ApplicationStatic.GameSeed.Value;
		}

		public void Exit()
		{
			Debug.Log("[GameMenu] Exit");
			UnityEngine.Application.Quit();
		}
	}
}
