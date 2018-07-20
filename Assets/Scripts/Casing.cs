using UnityEngine;

public class Casing : MonoBehaviour {

	public Transform leftAngleLimit;
	public Transform rightAngleLimit;
	public float maxForce;
	public float minForce;
	public float maxAngularVelocity;
	public float minAngularVelocity;
	public int maxLayer;
	public int minLayer;

	public GameObject player;

	public float remainingTime;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		
		float myXforce = Random.Range(leftAngleLimit.up.x, rightAngleLimit.up.x);
		float myYforce = Random.Range(leftAngleLimit.up.y, rightAngleLimit.up.y);
		float myAngularVelocity = Random.Range(minAngularVelocity, maxAngularVelocity);

		if (player.GetComponent<Player>().isLookingLeft)
		{
			myYforce = myYforce * -1;
		}

		Vector2 myVector = new Vector2(myXforce, myYforce);
		GetComponent<Rigidbody2D>().AddForce(myVector * Random.Range(minForce, maxForce));
		GetComponent<Rigidbody2D>().angularVelocity = myAngularVelocity;
	}

	private void Update()
	{
		remainingTime -= Time.deltaTime;

		if(remainingTime <= 0)
		{
			Destroy(gameObject);
		}
	}
}
