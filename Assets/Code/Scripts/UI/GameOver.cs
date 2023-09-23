public class GameOver : ViewBase {
	public void ReturnToMainMenu () {
		Events.UI.OnReturnToMenu.Invoke ();
	}
}
