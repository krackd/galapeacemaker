using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour {

	public UnityEvent OnDeath;

	public int MaxHP = 100;
	public bool IsInvincible = false;

	public bool IsDead { get { return hp <= 0; } }


	private int hp;

	// Use this for initialization
	void Start () {
		hp = MaxHP;
	}

	private void OnTriggerEnter(Collider other)
	{
		Projectile projectile = other.gameObject.GetComponent<Projectile>();
		if (projectile != null)
		{
			Hurt(projectile.Damage);
		}
	}

	public void Hurt(int amount)
	{
		if (IsInvincible || IsDead)
		{
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
