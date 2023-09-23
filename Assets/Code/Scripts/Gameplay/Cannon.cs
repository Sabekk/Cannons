using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour, ObjectPool.IPoolable {
	[SerializeField] Bullet bullet;
	[SerializeField] GameObject body;
	[SerializeField] CircleCollider2D cannonCollider;
	[SerializeField] Transform rilfe;
	[SerializeField] Color[] lifeColors;
	[SerializeField] Image bodyImage;

	Transform parent;
	public float Radius => cannonCollider != null ? cannonCollider.radius : 0;
	public CircleCollider2D Collider => cannonCollider;
	public float Width => (transform as RectTransform).rect.width * currentScale;
	public ObjectPool.PoolObject Poolable { get; set; }

	float rotationSpeed = 10;

	float rotationTime = 1;
	float shotingTime = 1;

	float nextRotation;

	float rotationTimer;
	float shotingTimer;

	float currentScale;
	Quaternion nextRotate = new Quaternion (0, 0, 0, 0);

	int initLives = 3;
	int lives;
	bool recovering;
	float recoverTime = 2;
	float recorerTimer;

	private void OnEnable () {
		Events.Gameplay.OnCannonGetHit += OnGetHit;
		Events.Gameplay.OnGameOver += OnCannonDestroyed;
	}
	private void OnDisable () {
		Events.Gameplay.OnGameOver -= OnCannonDestroyed;
		Events.Gameplay.OnCannonGetHit -= OnGetHit;
	}
	private void Update () {
		if (!recovering) {
			rotationTimer += Time.deltaTime;
			shotingTimer += Time.deltaTime;
			if (rotationTimer >= nextRotation) {
				RotateCannon ();
			}
			if (shotingTimer >= shotingTime)
				Shot ();

			transform.rotation = Quaternion.Lerp (transform.rotation, nextRotate, rotationSpeed * Time.deltaTime);

		} else {
			recorerTimer += Time.deltaTime;
			if (recorerTimer >= recoverTime) {
				recorerTimer = 0;
				recovering = false;
				ChangeBodyStatus (true);
			}
		}
	}

	public void Initialzie (float scale) {
		parent = GetComponentInParent<Transform> ();
		recovering = false;
		recorerTimer = 0;
		shotingTimer = 0;
		lives = initLives;
		transform.localScale = Vector3.one * scale;
		currentScale = scale;
		nextRotation = Random.Range (0.0f, rotationTime);
		CheckNextRotate ();
		CheckCurrentColor ();
	}
	void RotateCannon () {
		rotationTimer = 0;
		nextRotation = Random.Range (0.0f, rotationTime);
		nextRotate = Quaternion.Euler (Vector3.forward * Random.Range (0.0f, 360.0f));
		CheckNextRotate ();
	}

	void CheckNextRotate () {
		if (nextRotate.x + nextRotate.y + nextRotate.z + nextRotate.w == 0)
			nextRotate = Quaternion.identity;
	}
	void Shot () {
		shotingTimer = 0;
		Bullet bullet = ObjectPool.Instance.GetFromPool ("Bullet").GetComponent<Bullet> ();
		bullet.transform.SetParent (parent);
		bullet.Initialize (currentScale, gameObject, rilfe.transform);
	}
	void OnGetHit (GameObject hittedObject) {
		if (hittedObject != gameObject)
			return;
		if (recovering)
			return;
		if (lives <= 0)
			return;

		lives--;
		if (lives <= 0) {
			OnCannonDestroyed ();
			Events.Gameplay.OnCannonDestoyed.Invoke ();
		} else {
			ChangeBodyStatus (false);
			recovering = true;
		}
	}
	void OnCannonDestroyed () {
		ObjectPool.Instance.ReturnToPool (this);
	}

	void ChangeBodyStatus (bool isEnable) {
		body.SetActive (isEnable);
		cannonCollider.enabled = isEnable;
		if (isEnable)
			CheckCurrentColor ();
	}

	void CheckCurrentColor () {
		int indexOfColor = lives - 1;
		if (lifeColors.Length < lives)
			indexOfColor = lifeColors.Length - 1;
		bodyImage.color = lifeColors[indexOfColor];
	}

	public void AssignPoolable (ObjectPool.PoolObject poolable) {
		Poolable = poolable;
	}
}
