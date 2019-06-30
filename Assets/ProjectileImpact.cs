using UnityEngine;

public class ProjectileImpact : MonoBehaviour {

	public GameObject ImpactPrefab;
	
	private void OnCollisionEnter(Collision collision)
	{
		Projectile projectile = collision.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			ContactPoint hit = collision.contacts[0];
			//Vector3 dir = collision.transform.position - hit.point;
			//CreateImpact(hit.point, -projectile.Direction);
			CreateImpact(hit.point, -hit.normal); // TODO compute actual direction (to player)
		}
	}

	private void CreateImpact(Vector3 position, Vector3 dir)
	{
		GameObject impact = Instantiate(ImpactPrefab);
		impact.transform.position = position;
		impact.transform.rotation = Quaternion.LookRotation(dir);
	}
}
