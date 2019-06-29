using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyOnDeath))]
public class Asteroid : MonoBehaviour {

	public int NbDestuctions = 2;

	private DestroyOnDeath destroyOnDeath;

	private void Start()
	{
		destroyOnDeath = GetComponent<DestroyOnDeath>();
		
	}
}
