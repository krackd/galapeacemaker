using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class SpawnOnDeath : MonoBehaviour {

	[Header("Fragments")]
	public GameObject[] FragmentsPrefab;
	public Vector3 FragmentsScale = Vector3.one;
	public int NbFragments = 1;

	[Header("Position")]
	public bool ForceZ = true;
	public float PositionZ = 0;

	[Header("Offset")]
	public bool NormalizeDirection = false;
	public bool AddOffset = false;
	public Vector3 FragmentOffset = Vector3.zero;

	[Header("Expulsion")]
	public bool Expulse = false;
	public float FragmentExpulsionForce = 2f;

	// Use this for initialization
	void Start () {
		Health health = GetComponent<Health>();
		health.OnDeath.AddListener(OnDeath);
	}

	public void OnDeath()
	{
		CreateAllFragments();
	}

	private void CreateAllFragments()
	{
		if (FragmentsPrefab.Length <= 0)
		{
			return;
		}

		for (int i = 0; i < NbFragments; i++)
		{
			CreateOneFragments();
		}
	}

	private void CreateOneFragments()
	{
		int fi = Random.Range(0, FragmentsPrefab.Length - 1);
		GameObject fragment = Instantiate(FragmentsPrefab[fi]);

		float angle = Random.Range(0, 2 * Mathf.PI);
		//Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
		Vector3 dir = Random.insideUnitSphere;
		dir.z = 0;
		if (NormalizeDirection)
		{
			dir.Normalize();
		}

		fragment.transform.localScale = mult(fragment.transform.localScale, FragmentsScale);

		Vector3 pos = transform.position;
		if (ForceZ)
		{
			pos.z = PositionZ;
		}
		fragment.transform.position = pos;

		if (AddOffset)
		{
			Vector3 eo = FragmentOffset;
			Vector3 offset = new Vector3(dir.x * eo.x, dir.y * eo.y, dir.z * eo.z);
			fragment.transform.position += offset;
		}

		if (Expulse)
		{
			DoExpulsion(fragment, dir);
		}
	}

	private Vector3 mult(Vector3 a, Vector3 b)
	{
		return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
	}

	private void DoExpulsion(GameObject fragment, Vector3 dir)
	{
		Rigidbody rb = fragment.GetComponent<Rigidbody>();
		if (rb != null)
		{
			Vector3 force = dir * FragmentExpulsionForce;
			rb.AddForce(force);
		}
	}
}
