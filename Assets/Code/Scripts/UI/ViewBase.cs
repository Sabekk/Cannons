using UnityEngine;

public abstract class ViewBase : MonoBehaviour {
	public const string NAV_up = "nav_Up";
	public const string NAV_down = "nav_down";
	public const string NAV_left = "nav_left";
	public const string NAV_right = "nav_right";

	private void Start () {
		Initialize ();
	}
	protected void OnEnable () {
		OnActivate ();
		Events.UI.NavigateUp += OnNavigateUp;
		Events.UI.NavigateDown += OnNavigateDown;
		Events.UI.NavigateLeft += OnNavigateLeft;
		Events.UI.NavigateRIght += OnNavigateRight;
	}
	protected void OnDisable () {
		OnDeactivate ();
		Events.UI.NavigateUp -= OnNavigateUp;
		Events.UI.NavigateDown -= OnNavigateDown;
		Events.UI.NavigateLeft -= OnNavigateLeft;
		Events.UI.NavigateRIght -= OnNavigateRight;
	}
	public void OnNavigate () {

	}
	public virtual void OnActivate () {

	}
	public virtual void OnDeactivate () {

	}

	protected virtual void Initialize () {

	}

	public virtual void OnNavigate(string type) {

	}

	void OnNavigateUp () {
		OnNavigate (NAV_up);
	}
	void OnNavigateDown () {
		OnNavigate (NAV_down);
	}
	void OnNavigateLeft () {
		OnNavigate (NAV_left);
	}
	void OnNavigateRight () {
		OnNavigate (NAV_right);
	}
}
