using System.Collections.Generic;
using UnityEngine;

public class MainMenu : ViewBase {
	List<ModeButton> buttons;
	StartButton startButton;

	GameplayManager.Mode choosenMode;
	public bool AwailableToStart => choosenMode != GameplayManager.Mode.none;

	ModeButton selectedButton;
	private void Awake () {
		buttons = new List<ModeButton> ();
		buttons.AddRange (GetComponentsInChildren<ModeButton> ());
		startButton = GetComponentInChildren<StartButton> ();
	}

	private void OnDestroy () {
		buttons.Clear ();
	}
	protected override void Initialize () {
		foreach (var button in buttons) {
			button.Initialize ();
		}
		if (startButton)
			startButton.Initialize ();
	}
	public override void OnActivate () {
		base.OnActivate ();
		Refresh ();

		Events.UI.OnModeChange += OnModeChange;
	}
	public override void OnDeactivate () {
		base.OnDeactivate ();
		choosenMode = GameplayManager.Mode.none;
		foreach (var button in buttons)
			button.ToggleTransition (false);
		
		Events.UI.OnModeChange -= OnModeChange;
	}

	void OnModeChange (GameplayManager.Mode newMode) {
		choosenMode = newMode;
		Refresh ();
	}

	public void StartGame () {
		if (!AwailableToStart)
			return;
		else {
			Events.Gameplay.OnSetGameplayMode.Invoke (choosenMode);
			selectedButton = null;
			Events.Gameplay.OnStartGame.Invoke ();
		}
	}

	void Refresh () {
		foreach (var button in buttons) 
			button.ToggleTransition (button.Mode == choosenMode);
		
		if (startButton)
			startButton.ToggleTransition (AwailableToStart);
	}

	void CycleButtons (int dir) {
		int currentIndex = 0;
		if (selectedButton == null) {
			if (buttons.Count == 0)
				return;
			else
				selectedButton = buttons[0];
		} else {
			for (int i = 0; i < buttons.Count; i++) {
				if (buttons[i] == selectedButton) {
					currentIndex = i;
					break;
				}
			}
			currentIndex += dir;
			if (currentIndex >= buttons.Count)
				currentIndex = 0;
			if (currentIndex < 0)
				currentIndex = buttons.Count - 1;
		}

		selectedButton = buttons[currentIndex];
		selectedButton.OnClick ();
	}
}
