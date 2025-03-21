using BenjaminHamon.Solitaire.UnityClient.Runtime.Content;
using BenjaminHamon.Solitaire.UnityExtensions.Runtime;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace BenjaminHamon.Solitaire.UnityClient.Runtime
{
	public class MainMenu : MonoBehaviour
	{
		public MainMenu()
		{
			SeedInputMaxLength = Int32.MaxValue.ToString().Length - 1;
			SeedMaxValue = Convert.ToInt32(Math.Pow(10, SeedInputMaxLength)) - 1;
		}

		private readonly int SeedInputMaxLength;
		private readonly int SeedMaxValue;

		[SerializeField]
		private UIDocument UIDocument;

		[SerializeField]
		private bool RandomSeedOnStart = true;

		[SerializeField]
		private int Seed;

		public void Start()
		{
			ApplicationStatic.Application.AssetLoader.LoadBundle(AssetBundleNames.Interface);

			if (RandomSeedOnStart)
			{
				SetRandomSeed();
			}

			TextField seedInput = UIDocument.rootVisualElement.Query<TextField>("SeedInput");
			seedInput.value = Seed.ToString();

			Button lastSeedButton = UIDocument.rootVisualElement.Query<Button>("LastSeedButton");
			lastSeedButton.SetEnabled(ApplicationStatic.Application.GameSeed != null);
		}

		public void ApplyStyles(IEnumerable<StyleSheet> styleSheetCollection)
		{
			UIDocument.ApplyStyles(styleSheetCollection);
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
			ApplicationStatic.Application.GameSeed = Seed;
			SceneManager.LoadScene("GameScene");
		}

		private void HandleSeedChanged(ChangeEvent<string> e)
		{
			string valueAsString = e.newValue;

			if (valueAsString.Length <= SeedInputMaxLength)
			{
				valueAsString = Regex.Replace(e.newValue, @"[^0-9]+", "");
				Seed = valueAsString != "" ? Convert.ToInt32(valueAsString) : 0;

				TextField seedInput = (TextField)e.target;
				seedInput.value = Seed.ToString();
			}
			else
			{
				TextField seedInput = (TextField)e.target;
				seedInput.value = String.IsNullOrEmpty(e.previousValue) ? Seed.ToString() : e.previousValue;
				seedInput.cursorIndex -= valueAsString.Length - SeedInputMaxLength;
				seedInput.textSelection.SelectNone();
			}
		}

		private void SetRandomSeed()
		{
			System.Random random = new System.Random();
			Seed = random.Next(SeedMaxValue + 1);

			TextField seedInput = UIDocument.rootVisualElement.Query<TextField>("SeedInput");
			seedInput.value = Seed.ToString();
		}

		private void SetLastSeed()
		{
			Seed = ApplicationStatic.Application.GameSeed.Value;

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
			ApplicationStatic.Application.AssetLoader.UnloadBundle(AssetBundleNames.Interface);
		}
	}
}
