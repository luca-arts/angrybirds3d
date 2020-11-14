using UnityEngine;

public class Wood : MonoBehaviour
{
	public GameObject WoodShatter;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 13.5f)
		{
			Destroy();
		}
	}

	private void Destroy()
	{
		GameObject shatter = Instantiate(WoodShatter, transform.position, Quaternion.identity);
		Destroy(shatter, 2);
		Destroy(gameObject);
	}
}