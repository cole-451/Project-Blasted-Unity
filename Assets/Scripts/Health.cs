using UnityEngine;

public class Health : MonoBehaviour, IHealable, IDamagable
{
	[SerializeField] private float maxHealth = 100;
	[SerializeField] private bool destroyOnDeath = true;

	private float health;

	public bool IsDead { get => health <= 0; }

	void Awake()
	{
		health = maxHealth;
	}

	public void OnDamage(float damage)
	{
		health -= damage;
		health = Mathf.Clamp(health, 0, maxHealth);

		if (IsDead && destroyOnDeath)
		{
			Destroy(gameObject);
		}
	}

	public void OnHeal(float heal)
	{
		health += heal;
		health = Mathf.Clamp(health, 0, maxHealth);
	}
}
