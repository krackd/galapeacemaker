using UnityEngine;

public class SpawnOnImpact : MonoBehaviour {

	public GameObject ObjectPrefab;
	public float ExpulsionForce = 2f;

	[Range(0, 100)]
	public float ProjectilePercentChanceOfSpwan = 10f;

	[Range(0, 100)]
	public float BeamPercentChanceOfSpwan = 20f;

	private void OnCollisionEnter(Collision collision)
	{
		Projectile projectile = collision.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			float rand = GetRand();
			if (rand <= ProjectilePercentChanceOfSpwan)
			{
				ContactPoint hit = collision.contacts[0];
				CreateObject(hit.point, -hit.normal);
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		Beam beam = other.gameObject.GetComponentInParent<Beam>();
		if (beam != null)
		{
			float rand = Random.value * 100f;
			if (rand <= BeamPercentChanceOfSpwan)
			{
				RaycastHit? hit = beam.GetHit(gameObject);
				if (hit.HasValue)
				{
					CreateObject(hit.Value.point, hit.Value.normal);
				}
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

	private static float GetRand()
	{
		return Random.value * 100f;
	}
}
