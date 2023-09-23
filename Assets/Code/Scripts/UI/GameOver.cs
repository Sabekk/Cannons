public class GameOver : ViewBase {
	public void ReturnToMainMenu () {
		Events.UI.OnReturnToMenu.Invoke ();
	}

	public override void OnNavigate (string type) {
		base.OnNavigate (type);
		if (type == NAV_down)
			ReturnToMainMenu ();
	}
}
