using UnityEngine;

public class MainUI : MonoBehaviour {
	[SerializeField] GameObject mainMenu;
	[SerializeField] GameObject gameOverView;
	[SerializeField] GameObject battleField;

	private void Awake () {
		Events.Gameplay.OnStartGame += OnStartNewGame;
		Events.Gameplay.OnGameOver += OnGameOver;
		Events.UI.OnReturnToMenu += OnReturnToMenu;
	}

	private void OnDestroy () {
		Events.Gameplay.OnStartGame -= OnStartNewGame;
		Events.Gameplay.OnGameOver -= OnGameOver;
		Events.UI.OnReturnToMenu -= OnReturnToMenu;
	}

	private void Start () {
		mainMenu.SetActive (true);
		gameOverView.SetActive (false);
		battleField.SetActive (false);
	}

	void OnStartNewGame () {
		mainMenu.SetActive (false);
		battleField.SetActive (true);
	}

	void OnGameOver () {
		gameOverView.SetActive (true);
		battleField.SetActive (false);
	}

	void OnReturnToMenu () {
		mainMenu.SetActive (true);
		gameOverView.SetActive (false);
	}
}
