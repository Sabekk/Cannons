using UnityEngine;

public class GameplayEvents {
	public Events.Event<GameplayManager.Mode> OnStartGame = new Events.Event<GameplayManager.Mode> ();
	public Events.Event OnGameOver = new Events.Event ();
}
