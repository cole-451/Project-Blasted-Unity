
using UnityEngine;

public class HitBox : Damager
{
	[SerializeField] float activeTime = 0;

	private void OnEnable()
	{
		if (activeTime > 0) Invoke(nameof(Deactivate), activeTime);
	}

	void Deactivate()
	{
		gameObject.SetActive(false);
	}
}