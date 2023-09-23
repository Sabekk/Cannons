using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ModeButton : UIButton {
	[SerializeField] TMP_Text description;
	[SerializeField] Image cubeImage;
	[SerializeField] Color cubeColor;
	[SerializeField] float choosenScale;
	[SerializeField] GameplayManager.Mode mode;

	public GameplayManager.Mode Mode => mode;
	public override void Initialize () {
		base.Initialize ();
		cubeImage.color = cubeColor;
		description.SetText (GameplayManager.Instance.GetModeCannonsCount (mode).ToString ());
		ToggleTransition (false);
	}

	public override void OnClick () {
		Events.UI.OnModeChange.Invoke (mode);
	}
	public override void ToggleTransition (bool state) {
		base.ToggleTransition (state);
		if (state == true)
			transform.localScale = Vector3.one * choosenScale;
		else
			transform.localScale = Vector3.one;
	}
}
