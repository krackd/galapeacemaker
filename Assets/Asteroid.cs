using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

	public int MaxHp = 100;
	private int hp;

	public int NbFragments = 3;
	public float MinScale = 1f;
	public float FragmentExpulsionForce = 2f;

	private Random rand = new Random();

	private void Start()
	{
		hp = MaxHp;
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
		Vector3 halfScale = transform.localScale * 0.5f;
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
		Vector3 ls = transform.localScale;
		Vector3 offset = new Vector3(dir.x * ls.x, dir.y * ls.y, dir.z * ls.z);
		fragment.transform.Translate(offset);

		Rigidbody rb = fragment.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.AddForce(Vector3.up * FragmentExpulsionForce);
		}
	}
}
