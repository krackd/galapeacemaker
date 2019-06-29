using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {

	public bool UseTrigger = true;
	public string Tag = "Player";
	public GameObject ToDestroy;

	private void OnTriggerEnter(Collider other)
	{
		if (UseTrigger && other.gameObject.CompareTag(Tag))
		{
			DoDestroy();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!UseTrigger && collision.gameObject.CompareTag(Tag))
		{
			DoDestroy();
		}
	}

	private void DoDestroy()
	{
		if (ToDestroy == null)
		{
			Destroy(gameObject);
		}
		else
		{
			Destroy(ToDestroy);
		}
	}
}
