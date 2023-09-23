using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoSingleton<GameplayManager> {
	public enum Mode { none, Cannons50, Cannons100, Cannons250, Cannons500 }
	public GameMode CurrentMode => currentMode;

	[SerializeField] GameMode[] gameMods;

	GameMode currentMode;
	int objectsInGame = 0;

	protected override void Awake () {
		base.Awake ();
		Events.Gameplay.OnSetGameplayMode += OnSetGameplayMode;
		Events.Gameplay.OnCannonDestoyed += OnCannonDestroyed;
	}
	private void OnDestroy () {
		Events.Gameplay.OnSetGameplayMode -= OnSetGameplayMode;
		Events.Gameplay.OnCannonDestoyed -= OnCannonDestroyed;
	}
	void OnSetGameplayMode (Mode mode) {
		objectsInGame = GetModeCannonsCount (mode);
		ChangeGameMode (mode);
	}
	void OnCannonDestroyed () {
		objectsInGame--;
		if (objectsInGame <= 1)
			Events.Gameplay.OnGameOver.Invoke ();
	}

	void ChangeGameMode (Mode mode) {
		foreach (var gameMode in gameMods)
			if (gameMode.mode == mode) {
				currentMode = gameMode;
				break;
			}
	}
	public int GetModeCannonsCount (Mode mode) {
		foreach (var gameMode in gameMods)
			if (gameMode.mode == mode) {
				return gameMode.cannonsCount;
			}
		return 0;
	}

	[System.Serializable]
	public struct GameMode {
		public Mode mode;
		public float cannonScale;
		public int cannonsCount;
	}
}
