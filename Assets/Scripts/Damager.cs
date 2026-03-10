using UnityEngine;

public class Damager : MonoBehaviour
{
	[SerializeField] string filterTag = string.Empty;
	[SerializeField] float damage;

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (string.IsNullOrEmpty(filterTag) || collider.gameObject.CompareTag(filterTag))
		{
			OnDamage(collider.gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (string.IsNullOrEmpty(filterTag) || collision.gameObject.CompareTag(filterTag))
		{
			OnDamage(collision.gameObject);
		}
	}

	private void OnDamage(GameObject target)
	{
		if (target.TryGetComponent<IDamagable>(out var damagable))
		{
			damagable.OnDamage(damage);
		}
	}
}
