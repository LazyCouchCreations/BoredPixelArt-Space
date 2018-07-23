using UnityEngine;

public class Pea : MonoBehaviour {

	public float speed;
	public Rigidbody2D rb;

	private void Start()
	{
		rb.velocity = transform.forward * speed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Ground")
		{
			Destroy(gameObject);
		}		
	}
}
