using UnityEngine;

public class GameplayEvents {
	public Events.Event OnStartGame = new Events.Event ();
	public Events.Event OnGameOver = new Events.Event ();
	public Events.Event<GameplayManager.Mode> OnSetGameplayMode = new Events.Event<GameplayManager.Mode> ();
	public Events.Event<int, float> OnSpawnCannons = new Events.Event<int, float> ();
	public Events.Event<GameObject> OnCannonGetHit = new Events.Event<GameObject> ();
	public Events.Event OnCannonDestoyed = new Events.Event ();
}
