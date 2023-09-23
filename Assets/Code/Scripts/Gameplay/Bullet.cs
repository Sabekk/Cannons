using UnityEngine;

public class Bullet : MonoBehaviour, ObjectPool.IPoolable {
	Rigidbody2D rb;
	GameObject owner;
	float speed = 250;
	public ObjectPool.PoolObject Poolable { get; set; }

	private void Awake () {
		rb = GetComponent<Rigidbody2D> ();
	}
	private void Update () {
		rb.velocity = transform.up * speed;
		if (IsOutOfScreen ())
			OnBulletDestroyed ();
	}
	private void OnEnable () {
		Events.Gameplay.OnGameOver += OnBulletDestroyed;
	}
	private void OnDisable () {
		Events.Gameplay.OnGameOver -= OnBulletDestroyed;
	}

	private void OnTriggerEnter2D (Collider2D collision) {
		if (collision.gameObject.tag == "Cannon") {
			if (collision.gameObject == owner)
				return;
			Events.Gameplay.OnCannonGetHit.Invoke (collision.gameObject);
			OnBulletDestroyed ();
		}
	}

	public void Initialize (float scale, GameObject owner, Transform rilfeTransform) {
		transform.localScale = Vector3.one * scale;
		this.owner = owner;
		transform.position = rilfeTransform.position;
		transform.rotation = rilfeTransform.rotation;
	}
	bool IsOutOfScreen () {
		return !Screen.safeArea.Contains (transform.position);
	}
	void OnBulletDestroyed () {
		ObjectPool.Instance.ReturnToPool (this);
	}
	public void AssignPoolable (ObjectPool.PoolObject poolable) {
		Poolable = poolable;
	}
}
