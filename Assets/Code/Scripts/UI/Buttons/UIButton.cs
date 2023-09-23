using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Button))]
public abstract class UIButton : MonoBehaviour {
	[SerializeField] Transition transition;
	[SerializeField] Image mainImage;
	protected Button button;

	protected virtual void Awake () {
		button = GetComponent<Button> ();
		button.onClick.RemoveAllListeners ();
		button.onClick.AddListener (OnClick);
	}
	public virtual void Initialize () {
		ToggleTransition (false);
	}
	public virtual void OnClick () {

	}
	public virtual void ToggleTransition (bool state) {
		mainImage.sprite = state ? transition.active : transition.inactive;
	}
	[System.Serializable]
	public struct Transition {
		public Sprite active;
		public Sprite inactive;
	}
}
