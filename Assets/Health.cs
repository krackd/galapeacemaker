using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public UnityEvent OnDeath;

	[Header("Health")]
	public int MaxHP = 100;
	public bool IsInvincible = false;

	[Header("Modifiers")]
	public float ProjectileDamageFactor = 1f;
	public float BeamDamageFactor = 1f;

	public bool IsDead { get { return hp <= 0; } }


	private int hp;

	// Use this for initialization
	void Start () {
		hp = MaxHP;
	}

	private void OnTriggerEnter(Collider other)
	{
		OnCollision(other.gameObject);
	}

	private void OnTriggerStay(Collider other)
	{
		OnCollision(other.gameObject);
	}

	private void OnCollisionEnter(Collision collision)
	{
		OnCollision(collision.gameObject);
	}

	private void OnCollisionStay(Collision collision)
	{
		OnCollision(collision.gameObject);
	}

	private void OnCollision(GameObject otherGo)
	{
		Projectile projectile = otherGo.GetComponent<Projectile>();
		if (projectile != null)
		{
			Debug.Log("Projectile hurts");
			Hurt((int)(projectile.Damage * ProjectileDamageFactor));
		}

		Beam beam = otherGo.GetComponentInParent<Beam>();
		if (beam != null && beam.CanHurt)
		{
			Debug.Log("Beam hurts");
			Debug.Log(gameObject.name + " hp: " + hp);
			Hurt((int)(beam.Damage * BeamDamageFactor));
			beam.StartCooldown();
		}
	}

	public void Hurt(int amount)
	{
		if (IsInvincible || IsDead)
		{
			Debug.Log("IsInvincible: " + IsInvincible + " / IsDead: " + IsDead);
			return;
		}

		hp = clamp(hp - amount);
		Debug.Log(gameObject.name + " hp: " + hp);
		if (IsDead)
		{
			Die();
		}
	}

	public void Heal(int amount)
	{
		hp = clamp(hp + amount);
	}

	public void Die()
	{
		OnDeath.Invoke();
	}

	private int clamp(int value)
	{
		return hp = (int)Mathf.Clamp(value, 0, MaxHP);
	}
}
