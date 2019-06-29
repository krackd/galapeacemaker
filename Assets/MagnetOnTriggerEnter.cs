using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetOnTriggerEnter : MonoBehaviour {

	public string TargetTag = "Player";
	public float MoveSpeed = 0.1f;

	private GameObject target;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(TargetTag))
		{
			target = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag(TargetTag))
		{
			target = null;
		}
	}

	private void Update()
	{
		if (target != null)
		{
			Vector3 diff = target.transform.position - transform.position;
			Vector3 lerp = Vector3.Lerp(Vector3.zero, diff, MoveSpeed * Time.deltaTime * 1000);
			transform.Translate(lerp);
		}
	}
}
