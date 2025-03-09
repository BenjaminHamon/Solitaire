using BenjaminHamon.Solitaire.Model;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Controllers;
using BenjaminHamon.Solitaire.UnityClient.Runtime.Views;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
    public class GameScene : MonoBehaviour
    {
		[SerializeField]
		private GameObject GamePrefab;
		[SerializeField]
		private InputController Controller;
		[SerializeField]
		private GameConfiguration Configuration = null;
		[SerializeField]
		private int Seed = 0;

		private GameView gameView;
		private Game game;

		public void Start()
		{
			ApplicationStatic.Application.AssetLoader.LoadBundle(AssetBundleNames.Cards);
			ApplicationStatic.Application.AssetLoader.LoadBundle(AssetBundleNames.Prefabs);

			if (ApplicationStatic.Application.GameSeed != null)
			{
				Seed = ApplicationStatic.Application.GameSeed.Value;
			}

			UnityEngine.Debug.Log(String.Format("[GameScene] Starting new game (Seed: {0})", Seed));

			game = ApplicationStatic.Application.NewGame(Configuration, Seed);
			game.Initialize();

			GameObject newGameObject = Instantiate(GamePrefab);
			newGameObject.name = "Game";

			gameView = newGameObject.GetComponent<GameView>();
			gameView.Model = game;
			gameView.SetUp();

			Controller.Game = gameView;

			Screen.orientation = ScreenOrientation.LandscapeLeft;
		}

		public virtual void OnEnable()
		{
			if (game != null)
			{
				game.EnableEventListeners();
			}

			if (gameView != null)
			{
				gameView.Completed += Exit;
			}
		}

		public virtual void OnDisable()
		{
			if (gameView != null)
			{
				gameView.Completed -= Exit;
			}

			if (game != null)
			{
				game.DisableEventListeners();
			}
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Exit();
			}
		}

		public void OnDestroy()
		{
			if (game != null)
			{
				game = null;

				ApplicationStatic.Application.EndGame();
			}

			ApplicationStatic.Application.AssetLoader.UnloadBundle(AssetBundleNames.Cards);
			ApplicationStatic.Application.AssetLoader.UnloadBundle(AssetBundleNames.Prefabs);

			Screen.orientation = ScreenOrientation.AutoRotation;
		}

		private void Exit()
		{
			Debug.Log("[GameScene] Exiting to menu");
			SceneManager.LoadScene("MenuScene");
		}
	}
}
