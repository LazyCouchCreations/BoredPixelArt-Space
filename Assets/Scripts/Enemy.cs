using UnityEngine;

public class Enemy : MonoBehaviour {

	public Transform[] wayPoints;
	public Transform feetPos;
	public Transform lineOfSightPos;
	public float lineOfSightDistance;
	public Transform jumpRight;
	public float checkRadius;
	public LayerMask whatIsGround;
	public LayerMask whatIsLineOfSight;
	private Rigidbody2D rb;
	public float mySpeed;
	public float speed;
	public float jumpForce;
	public float whiskerLength;
	public bool isGrounded;
	public bool isJumping;
	public bool isGoingRight;
	public bool isAggressive;
	public int currentWayPoint;
	public GameObject player;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		//if (wayPoints[0] == null)
		//{
		//	wayPoints[0] = player.transform;
		//}
		currentWayPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
		LookForThePlayer();
	}

	private void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

		Debug.DrawRay(transform.position, transform.right * whiskerLength, Color.cyan);
		
		if(transform.position.x < wayPoints[currentWayPoint].position.x)
		{
			isGoingRight = true;
		}
		else
		{
			isGoingRight = false;
		}

		if (isGoingRight)
		{
			mySpeed = speed;
			transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
		}
		else
		{
			mySpeed = -speed;
			transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 180, 0));
		}

		if (isGrounded && !isJumping)
		{
			rb.velocity = new Vector2(mySpeed, rb.velocity.y);
		}

		RaycastHit2D rightWhiskerHit = Physics2D.Raycast(transform.position, transform.right, whiskerLength, whatIsGround);
		if (isGrounded && rightWhiskerHit)
		{
			rb.velocity = jumpRight.up * jumpForce;
		}				
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.transform == wayPoints[currentWayPoint])
		{
			currentWayPoint = FindNextWaypoint(currentWayPoint);
		}
	}

	private int FindNextWaypoint(int currentWP)
	{
		if (currentWP < wayPoints.Length - 1)
		{
			return currentWP + 1;
		}
		else
		{
			return 0;
		}
	}

	private void LookForThePlayer()
	{
		lineOfSightPos.LookAt(player.transform);
		Debug.DrawRay(lineOfSightPos.position, lineOfSightPos.forward * lineOfSightDistance, Color.red);
		RaycastHit2D lineOfSightToPlayer = Physics2D.Raycast(lineOfSightPos.position, lineOfSightPos.forward, lineOfSightDistance, whatIsLineOfSight);

		if (lineOfSightToPlayer)
		{
			if (lineOfSightToPlayer.transform.tag == "Player")
			{

			}
		}
	}
}
