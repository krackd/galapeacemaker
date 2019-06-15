using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	public float Delay = 1f;

	void Start () {
		StartCoroutine(delayedDestroy(Delay));
	}

	private IEnumerator delayedDestroy(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}
}
