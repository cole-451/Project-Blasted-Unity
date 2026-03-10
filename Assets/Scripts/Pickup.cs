using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
	[SerializeField] string filterTag = string.Empty;

	protected abstract void OnPickup(GameObject other);

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (string.IsNullOrEmpty(filterTag) || collision.gameObject.CompareTag(filterTag))
		{
			OnPickup(collision.gameObject);
			// destroy self
			Destroy(gameObject);
		}
	}
}
