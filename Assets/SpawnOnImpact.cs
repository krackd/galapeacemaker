using UnityEngine;

public class SpawnOnImpact : MonoBehaviour {

	public GameObject ObjectPrefab;
	public float ExpulsionForce = 2f;

	[Range(0, 100)]
	public float PercentChanceOfSpwan = 50f;

	private void OnCollisionEnter(Collision collision)
	{
		Projectile projectile = collision.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			float rand = Random.value * 100f;
			if (rand <= PercentChanceOfSpwan)
			{
				ContactPoint hit = collision.contacts[0];
				CreateObject(hit.point, -hit.normal);
			}
		}
	}

	private void CreateObject(Vector3 position, Vector3 dir)
	{
		GameObject impact = Instantiate(ObjectPrefab);
		impact.transform.position = position;
		Rigidbody rb = impact.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.AddForce(dir * ExpulsionForce);
		}
	}
}
