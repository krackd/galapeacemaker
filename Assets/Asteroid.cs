using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

	public int HpPerScaleXUnit = 25;
	private int hp;

	public int NbFragments = 3;
	public float MinScale = 1f;
	public float FragmentExpulsionForce = 2f;

	private Rigidbody rb;

	private Random rand = new Random();

	private void Start()
	{
		int scale = (int)transform.localScale.x;
		hp = HpPerScaleXUnit * scale;
		rb = GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.mass *= scale;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Projectile projectile = other.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			DoDamage(projectile);
			CheckDestroy();
		}
	}

	private void DoDamage(Projectile projectile)
	{
		hp -= projectile.Damage;
	}

	private void CheckDestroy()
	{
		if (hp <= 0)
		{
			CreateAllFragments();
			Destroy(gameObject);
		}
	}

	private void CreateAllFragments()
	{
		Vector3 halfScale = transform.localScale * 1 / NbFragments;
		bool tooSmall = halfScale.x < MinScale || halfScale.y < MinScale || halfScale.z < MinScale;
		
		if (tooSmall)
		{
			return;
		}

		for (int i = 0; i < NbFragments; i++)
		{
			CreateOneFragment(halfScale);
		}
	}

	private void CreateOneFragment(Vector3 scale)
	{
		GameObject fragment = Instantiate(gameObject);

		float angle = Random.Range(0, 2 * Mathf.PI);
		Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

		fragment.transform.localScale = scale;
		Vector3 ls = transform.localScale * 1; // TODO manage with asteroid size instead of scale
		Vector3 offset = new Vector3(dir.x * ls.x, dir.y * ls.y, dir.z * ls.z);
		fragment.transform.position += offset;

		Rigidbody rb = fragment.GetComponent<Rigidbody>();
		if (rb != null)
		{
			Vector3 force = dir * FragmentExpulsionForce;
			//Debug.Log("force=" + force);
			rb.AddForce(force); // Does not work
			//rb.AddExplosionForce(FragmentExpulsionForce, transform.position, 1);
		}
	}
}
