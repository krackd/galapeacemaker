using UnityEngine;

public class Area : MonoBehaviour {

	public float Radius = 1f;

	public float SqrRadius { get { return Radius * Radius; } }

	private void OnDrawGizmosSelected()
	{
		Color color = Gizmos.color;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, Radius);
		Gizmos.color = color;
	}
}
