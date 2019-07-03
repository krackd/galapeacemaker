using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeapon : MonoBehaviour {

	private Beam beam;
	private Animator anim;

	// Use this for initialization
	void Start () {
		beam = GetComponentInChildren<Beam>();
		if (beam != null)
		{
			anim = beam.GetComponent<Animator>();
			beam.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire2") && beam != null && anim != null)
		{
			beam.gameObject.SetActive(true);
			beam.StartCooldown();
		}
		else if (Input.GetButtonUp("Fire2") && beam != null)
		{
			beam.gameObject.SetActive(false);
		}
	}
}
