using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector3 Direction = Vector3.up;
	public float Speed = 1f;

	public int Damage = 10;

	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate(Direction * Speed * Time.deltaTime * 1000f);
	}

	private void OnTriggerEnter(Collider other)
	{
		Destroy(gameObject);
	}
}
