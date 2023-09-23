using UnityEngine;

public abstract class ViewBase : MonoBehaviour {
	public const string Dpad_UP = "DpadUp";
	public const string Dpad_DOWN = "DpadDown";
	public const string Dpad_LEFT = "DpadLeft";
	public const string Dpad_RIGHT = "DpadRight";

	private void Start () {
		Initialize ();
	}
	protected void OnEnable () {
		OnActivate ();
	}
	protected void OnDisable () {
		OnDeactivate ();
	}
	public virtual void OnActivate () {

	}
	public virtual void OnDeactivate () {

	}

	protected virtual void Initialize () {

	}
}
