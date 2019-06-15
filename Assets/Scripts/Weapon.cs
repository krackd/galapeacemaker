using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public GameObject LaserPrefab;
	public int Damage = 25;

	public float FireCoolDown = 0.3f;

	private bool canFire = true;

	// Use this for initialization
	void Start () {
		if (LaserPrefab == null)
		{
			Debug.LogError("The laser prefab is not defined (LaserPrefab is null)!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Fire1") && canFire && LaserPrefab != null)
		{
			DoFire();
			canFire = false;
			StartCoroutine(fireCooldown(FireCoolDown));
		}
	}

	private void DoFire()
	{
		GameObject laser = Instantiate(LaserPrefab);
		laser.transform.position = transform.position;
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, transform.up);
		laser.transform.rotation = rot;

		Projectile projectile = laser.GetComponent<Projectile>();
		if (projectile != null)
		{
			projectile.Damage = Damage;
		}
	}

	private IEnumerator fireCooldown(float delay)
	{
		yield return new WaitForSeconds(delay);
		canFire = true;
	}
}
