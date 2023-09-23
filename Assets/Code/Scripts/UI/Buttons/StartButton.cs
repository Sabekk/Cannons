using UnityEngine;
public class StartButton : UIButton {
	[SerializeField] GameObject gpad;

	public override void ToggleTransition (bool state) {
		base.ToggleTransition (state);
		gpad.SetActive (state);
	}
}
