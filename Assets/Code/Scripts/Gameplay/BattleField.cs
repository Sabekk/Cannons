using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField : ViewBase {
	RectTransform battleField;
	List<Vector2> possiblePositions;

	float widthfOfField;
	float heightfOfField;
	float widthOffset = 15;
	float heightOffset = 15;

	private void Awake () {
		battleField = GetComponent<RectTransform> ();
		widthfOfField = battleField.rect.width;
		heightfOfField = battleField.rect.height;
	}
	public override void OnActivate () {
		base.OnActivate ();
		SpawnCannons ();
	}
	public override void OnDeactivate () {
		base.OnDeactivate ();
		possiblePositions.Clear ();
	}

	void SpawnCannons () {
		int count = GameplayManager.Instance.CurrentMode.cannonsCount;
		float scale = GameplayManager.Instance.CurrentMode.cannonScale;

		GeneratePossiblePositions (count);

		Vector2 spawnPoint = Vector2.zero;
		List<Vector2> neighbourPos = new List<Vector2> ();

		for (int i = 0; i < count; i++) {
			Cannon cannon = ObjectPool.Instance.GetFromPool ("Cannon").GetComponent<Cannon> ();
			cannon.transform.SetParent (transform);
			cannon.Initialzie (scale, transform);

			float space = cannon.Width;
			bool positionSet = false;

			while (!positionSet) {
				positionSet = true;

				if (possiblePositions.Count == 0) {
					Debug.LogError ("Missing positions");
					break;
				}

				int randomNumber = Random.Range (0, possiblePositions.Count);
				spawnPoint = possiblePositions[randomNumber];

				possiblePositions.RemoveAt (randomNumber);

				foreach (var pos in neighbourPos) {
					float distance = Vector2.Distance (pos, spawnPoint);
					if (distance < space) {
						positionSet = false;
						break;
					}
				}
				if (positionSet) {
					cannon.transform.localPosition = spawnPoint;
					neighbourPos.Add (spawnPoint);
				}
			}
		}
	}

	void GeneratePossiblePositions (int count) {
		if (possiblePositions == null)
			possiblePositions = new List<Vector2> ();
		if (count < 50)
			count = 50;
		int countToFind = count / 2;
		possiblePositions.Clear ();
		float baseWidth = widthfOfField / (countToFind);
		float startPointInWidth = -(widthfOfField / 2);
		float baseHeight = heightfOfField / (countToFind);
		float startPointInHeight = -(heightfOfField / 2);
		for (int i = 1; i < countToFind; i++) {
			for (int j = 1; j < countToFind; j++) {
				float possibleX = startPointInWidth + baseWidth * i + Random.Range (0.5f, widthOffset);
				float possibleY = startPointInHeight + baseHeight * j + Random.Range (0.5f, heightOffset);
				possiblePositions.Add (new Vector2 (possibleX, possibleY));
			}
		}
	}
}
