using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField]
		private UIDocument UIDocument;

		[SerializeField]
		private bool RandomSeedOnStart = true;

		[SerializeField]
		private int Seed;

		public void Start()
		{
			ApplicationStatic.AssetLoader.LoadBundle(AssetBundleNames.Interface);

			if (RandomSeedOnStart)
			{
				SetRandomSeed();
			}

			TextField seedInput = UIDocument.rootVisualElement.Query<TextField>("SeedInput");
			seedInput.value = Seed.ToString();

			Button lastSeedButton = UIDocument.rootVisualElement.Query<Button>("LastSeedButton");
			lastSeedButton.SetEnabled(ApplicationStatic.GameSeed != null);
		}

		public void ApplyStyles(IEnumerable<StyleSheet> styleSheetCollection)
		{
			UIDocument.rootVisualElement.styleSheets.Clear();

			foreach (StyleSheet styleSheet in styleSheetCollection)
			{
				UnityEngine.Debug.LogFormat(this, "[MainMenu] Applying style sheet '{0}'", styleSheet);
				UIDocument.rootVisualElement.styleSheets.Add(styleSheet);
			}
		}

		public void OnEnable()
		{
			if (UIDocument.rootVisualElement != null)
			{
				Button newSeedButton = UIDocument.rootVisualElement.Query<Button>("NewGameButton");
				newSeedButton.clicked += StartNewGame;

				TextField seedInput = UIDocument.rootVisualElement.Query<TextField>("SeedInput");
				seedInput.RegisterCallback<ChangeEvent<string>>(HandleSeedChanged);

				Button lastSeedButton = UIDocument.rootVisualElement.Query<Button>("LastSeedButton");
				lastSeedButton.clicked += SetLastSeed;

				Button randomSeedButton = UIDocument.rootVisualElement.Query<Button>("RandomSeedButton");
				randomSeedButton.clicked += SetRandomSeed;

				Button exitSeedButton = UIDocument.rootVisualElement.Query<Button>("ExitButton");
				exitSeedButton.clicked += Exit; 
			}
		}

		public void OnDisable()
		{
			if (UIDocument.rootVisualElement != null)
			{
				Button newSeedButton = UIDocument.rootVisualElement.Query<Button>("NewGameButton");
				newSeedButton.clicked -= StartNewGame;

				TextField seedInput = UIDocument.rootVisualElement.Query<TextField>("SeedInput");
				seedInput.UnregisterCallback<ChangeEvent<string>>(HandleSeedChanged);

				Button lastSeedButton = UIDocument.rootVisualElement.Query<Button>("LastSeedButton");
				lastSeedButton.clicked -= SetLastSeed;

				Button randomSeedButton = UIDocument.rootVisualElement.Query<Button>("RandomSeedButton");
				randomSeedButton.clicked -= SetRandomSeed;

				Button exitSeedButton = UIDocument.rootVisualElement.Query<Button>("ExitButton");
				exitSeedButton.clicked -= Exit;
			}
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				Exit();
			}
		}

		private void StartNewGame()
		{
			ApplicationStatic.GameSeed = Seed;
			SceneManager.LoadScene("GameScene");
		}

		private void HandleSeedChanged(ChangeEvent<string> e)
		{
			Seed = e.newValue != "" ? Convert.ToInt32(e.newValue) : 0;

			TextField seedInput = (TextField) e.target;
			seedInput.value = Seed.ToString();
		}

		private void SetRandomSeed()
		{
			System.Random random = new System.Random();
			Seed = random.Next();

			TextField seedInput = UIDocument.rootVisualElement.Query<TextField>("SeedInput");
			seedInput.value = Seed.ToString();
		}

		private void SetLastSeed()
		{
			Seed = ApplicationStatic.GameSeed.Value;

			TextField seedInput = UIDocument.rootVisualElement.Query<TextField>("SeedInput");
			seedInput.value = Seed.ToString();
		}

		private void Exit()
		{
			Debug.Log("[GameMenu] Exit");
			UnityEngine.Application.Quit();
		}

		public void OnDestroy()
		{
			ApplicationStatic.AssetLoader.UnloadBundle(AssetBundleNames.Interface);
		}
	}
}
