using UnityEngine;

public class gun : MonoBehaviour {

	private Vector2 worldMousePos;
	private Vector2 myFlatTransform;
	public Transform stockPos;
	public GameObject laserPointPrefab;
	private GameObject laserPoint;
	public LayerMask whatIsLaserHittable;

	public GameObject casingPrefab;
	public Transform casingSpawn;

	public GameObject player;

	void Start()
	{
		laserPoint = Instantiate(laserPointPrefab);
	}

	// Update is called once per frame
	void Update()
	{
		worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		myFlatTransform = new Vector2(transform.position.x, transform.position.y);
		transform.position = stockPos.position;
		transform.right = worldMousePos - myFlatTransform;

		Vector2 direction = (worldMousePos - myFlatTransform);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100, whatIsLaserHittable);

		if (hit)
		{
			laserPoint.GetComponent<SpriteRenderer>().enabled = true;
		}
		else
		{
			laserPoint.GetComponent<SpriteRenderer>().enabled = false;
		}

		laserPoint.transform.position = hit.point;

		Debug.DrawRay(transform.position, transform.right * 10f, Color.red);

		if (Input.GetButtonDown("Fire1"))
		{
			SpawnCasing();
		}

		if(worldMousePos.x - myFlatTransform.x < 0)
		{
			player.GetComponent<Player>().isLookingLeft = true;
		}
		else
		{
			player.GetComponent<Player>().isLookingLeft = false;
		}
	}

	void SpawnCasing()
	{
		Instantiate(casingPrefab, casingSpawn.position, transform.rotation);
	}
}
