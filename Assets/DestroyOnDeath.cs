using UnityEngine;

public class DestroyOnDeath : MonoBehaviour {

	[Header("Fragments")]
	public GameObject FragmentsPrefab;
	public Vector3 FragmentsScale = Vector3.one;
	public int NbFragments = 1;

	[Header("Offset")]
	public bool AddOffset = false;
	public Vector3 FragmentOffset = Vector3.zero;

	[Header("Expulsion")]
	public bool Expulse = false;
	public float FragmentExpulsionForce = 2f;

	public void OnDeath()
	{
		CreateAllFragments();
		Destroy(gameObject);
	}

	private void CreateAllFragments()
	{
		if (FragmentsPrefab == null)
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
		GameObject fragment = Instantiate(FragmentsPrefab);

		float angle = Random.Range(0, 2 * Mathf.PI);
		Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

		fragment.transform.localScale = FragmentsScale;
		fragment.transform.position = transform.position;

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

	private void DoExpulsion(GameObject fragment, Vector3 dir)
	{
		Rigidbody rb = fragment.GetComponent<Rigidbody>();
		if (rb != null)
		{
			Vector3 force = dir * FragmentExpulsionForce;
			rb.AddForce(force); // Does not work
								//rb.AddExplosionForce(FragmentExpulsionForce, transform.position, 1);
		}
	}
}
