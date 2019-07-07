﻿using System.Collections;
using UnityEngine;

public class SpawnOnImpact : MonoBehaviour {

	public GameObject ObjectPrefab;
	public int NbResources = 500;
	private int currentNb = 0;
	public bool IsUnlimited = true;
	public bool IsDepleted { get { return !IsUnlimited && currentNb <= 0; } }

	public float ExpulsionForce = 1500f;

	[Header("Projectile")]
	[Range(0, 100)]
	public float ProjectilePercentChanceOfSpwan = 0f;

	[Header("Beam")]
	public bool IsBeamRandom = false;
	public float SpawnCooldown = 1f;
	private bool canSpawn = true;
	[Range(0, 100)]
	public float BeamPercentChanceOfSpwan = 2f;

	private void Start()
	{
		currentNb = NbResources;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (IsDepleted)
		{
			return;
		}

		OnProjectileImpact(collision);
	}

	private void OnProjectileImpact(Collision collision)
	{
		Projectile projectile = collision.gameObject.GetComponent<Projectile>();
		ContactPoint hit = collision.contacts[0];
		Vector3 position = hit.point;
		Vector3 direction = -hit.normal;
		if (projectile != null)
		{
			onRandom(position, direction, ProjectilePercentChanceOfSpwan);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (IsDepleted)
		{
			return;
		}

		OnBeamImpact(other);
	}

	private void OnBeamImpact(Collider other)
	{
		Beam beam = other.gameObject.GetComponentInParent<Beam>();
		if (beam != null)
		{
			RaycastHit? hit = beam.GetHit(gameObject);
			if (hit.HasValue)
			{
				Vector3 position = hit.Value.point;
				Vector3 direction = hit.Value.normal;
				if (IsBeamRandom)
				{
					onRandom(position, direction, BeamPercentChanceOfSpwan);
				}
				else
				{
					onPeriodic(position, direction);
				}
			}
		}
	}

	private void CreateObject(Vector3 position, Vector3 dir)
	{
		GameObject impact = Instantiate(ObjectPrefab);
		impact.transform.position = position;
		Rigidbody rb = impact.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.AddForce(dir * ExpulsionForce);
		}
		currentNb--;
	}

	private static float GetRand()
	{
		return Random.value * 100f;
	}

	private void DoSpawn(Vector3 position, Vector3 direction)
	{
		CreateObject(position, direction);
	}

	private void onRandom(Vector3 position, Vector3 direction, float percentChance)
	{
		float rand = GetRand();
		if (rand <= percentChance)
		{
			DoSpawn(position, direction);
		}
	}

	private void onPeriodic(Vector3 position, Vector3 direction)
	{
		if (canSpawn)
		{
			DoSpawn(position, direction);
			StartCoroutine(spawnCooldown(SpawnCooldown));
		}
	}

	private IEnumerator spawnCooldown(float cd)
	{
		canSpawn = false;
		yield return new WaitForSeconds(cd);
		canSpawn = true;
	}
}
