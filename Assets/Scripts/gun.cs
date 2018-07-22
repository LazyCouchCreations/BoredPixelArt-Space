using UnityEngine;

public class gun : MonoBehaviour {

	private Vector2 worldMousePos;
	private Vector2 myFlatTransform;
	public Transform stockPos;
	public GameObject laserPointPrefab;
	private GameObject laserPoint;
	public LayerMask whatIsLaserHittable;
	public GameObject bloodPrefab;

	public GameObject casingPrefab;
	public Transform casingSpawn;

	public GameObject player;

	private AudioClip pew;
	private AudioSource audio;

	void Start()
	{
		laserPoint = Instantiate(laserPointPrefab);
		audio = GetComponent<AudioSource>();
		pew = audio.clip;
	}

	// Update is called once per frame
	void Update()
	{
		worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		myFlatTransform = new Vector2(transform.position.x, transform.position.y);
		transform.position = stockPos.position;
		transform.right = worldMousePos - myFlatTransform;

		Vector2 direction = (worldMousePos - myFlatTransform);
		//ContactFilter2D filter = new ContactFilter2D();
		//filter.SetLayerMask(whatIsLaserHittable);
		//RaycastHit2D[] hits = new RaycastHit2D[]();
		//Physics2D.Raycast(transform.position, direction, filter.layerMask, hits);

		RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 100f, whatIsLaserHittable);

		if (hits.Length != 0)
		{
			laserPoint.GetComponent<SpriteRenderer>().enabled = true;
			laserPoint.transform.position = hits[0].point;
		}
		else
		{
			laserPoint.GetComponent<SpriteRenderer>().enabled = false;
		}

		

		Debug.DrawRay(transform.position, transform.right * 10f, Color.red);

		if (Input.GetButtonDown("Fire1"))
		{
			SpawnCasing();
			audio.pitch = Random.Range(.9f, 1.2f);
			audio.PlayOneShot(pew);
			if(hits[0].collider.tag == "Enemy")
			{
				GameObject blood = Instantiate(bloodPrefab, new Vector3(hits[0].point.x, hits[0].point.y, 0), transform.rotation);
				blood.transform.DetachChildren();
				Destroy(blood);
			}
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
