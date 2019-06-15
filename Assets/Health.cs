using UnityEngine;

public class Health : MonoBehaviour {

	public int MaxHP = 100;
	public bool IsInvincible = false;

	public bool IsDead { get { return hp <= 0; } }


	private int hp;

	// Use this for initialization
	void Start () {
		hp = MaxHP;
	}
	
	public void Hurt(int amount)
	{
		if (IsInvincible)
		{
			return;
		}

		hp = clamp(hp - amount);
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

	}

	private int clamp(int value)
	{
		return hp = (int)Mathf.Clamp(value, 0, MaxHP);
	}
}
