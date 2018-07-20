using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D rb;

	//running
	public float horSpeed;
	public float vertSpeed;
	private float moveInput;
	public float whiskerLength;
	public Transform leftWhisker;
	public Transform rightWhisker;

	//jumping
	public Transform feetPos;
	private bool jumpInput;
	public float checkRadius;
	public bool isGrounded;
	public LayerMask whatIsGround;
	public float groundJumpForce;
	public float airHoverForce;
	public float upForce;
	public float maxAirHoverTime;
	public float airHoverTime;

	//facing
	public bool isLookingLeft;
	public GameObject gun;
	public GameObject body;


	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		moveInput = Input.GetAxis("Horizontal");

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			jumpInput = true;
			upForce = groundJumpForce;
		}
		

		//if (Input.GetButtonUp("Jump"))
		//{
		//jumpInput = false;
		//}


		if (isLookingLeft)
		{
			LookLeft();
		}
		else
		{
			LookRight();
		}
	}

	private void FixedUpdate()
	{
		//detect slopes with whiskers
		RaycastHit2D leftWhiskerHit = Physics2D.Raycast(leftWhisker.position, leftWhisker.up, whiskerLength, whatIsGround);
		Debug.DrawRay(leftWhisker.position, leftWhisker.up * whiskerLength, Color.magenta);
		RaycastHit2D rightWhiskerHit = Physics2D.Raycast(rightWhisker.position, rightWhisker.right, whiskerLength, whatIsGround);
		Debug.DrawRay(rightWhisker.position, rightWhisker.right * whiskerLength, Color.magenta);
		if (rightWhiskerHit)
		{
			//vertSpeed = moveInput * horSpeed/1.5f;
			rb.velocity = rightWhisker.up * moveInput * horSpeed;
		}else if (leftWhiskerHit)
		{
			//vertSpeed = -moveInput * horSpeed / 1.5f;
			rb.velocity = leftWhisker.right * -moveInput * horSpeed;
		}
		else
		{
			rb.velocity = new Vector2(moveInput * horSpeed, rb.velocity.y);
		}

		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		if (isGrounded)
		{
			airHoverTime = maxAirHoverTime;
		}

		if (jumpInput && airHoverTime >= 0)
		{
			Vector2 halfwayVector = (rb.velocity + Vector2.up * upForce);
			rb.velocity = halfwayVector;
			jumpInput = false;
		}		
	}

	private void LookLeft()
	{
		body.transform.SetPositionAndRotation(body.transform.position, Quaternion.Euler(0, 180, 0));
		gun.GetComponent<SpriteRenderer>().flipY = true;
	}

	private void LookRight()
	{
		body.transform.SetPositionAndRotation(body.transform.position, Quaternion.Euler(0, 0, 0));
		gun.GetComponent<SpriteRenderer>().flipY = false;
	}
}
