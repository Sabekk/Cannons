using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoSingleton<GameplayManager> {
	public enum Mode { none, Cannons50, Cannons100, Cannons250, Cannons500 }
	[SerializeField] GameMode[] gameMods;
	
	GameMode currentMode;

	int objectsInGame = 0;

	public int GetModeCannonsCount (Mode mode) {
		foreach (var gameMode in gameMods)
			if (gameMode.mode == mode) {
				return gameMode.cannonsCount;
			}
		return 0;
	}

	[System.Serializable]
	struct GameMode {
		public Mode mode;
		public float cannonScale;
		public int cannonsCount;
	}
}
