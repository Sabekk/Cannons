using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour, InputBinds.IUIActions {
	static InputBinds _controll;
	public static InputBinds Input {
		get {
			if (_controll == null)
				_controll = new InputBinds ();
			return _controll;
		}
	}
	private void Awake () {
		Input.UI.SetCallbacks (this);
	}
	void OnEnable () {
		Input.Enable ();
	}
	void OnDisable () {
		Input.Disable ();
	}
	public void OnDpadDown (InputAction.CallbackContext context) {
		if (context.performed)
			Events.UI.NavigateDown.Invoke ();
	}

	public void OnDpadLeft (InputAction.CallbackContext context) {
		if (context.performed)
			Events.UI.NavigateLeft.Invoke ();
	}

	public void OnDpadRight (InputAction.CallbackContext context) {
		if (context.performed)
			Events.UI.NavigateRIght.Invoke ();
	}

	public void OnDpadUp (InputAction.CallbackContext context) {
		if (context.performed)
			Events.UI.NavigateUp.Invoke ();
	}
}
