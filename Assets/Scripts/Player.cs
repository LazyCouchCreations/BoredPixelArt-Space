using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D rb;

	//running
	public float speed;
	private float moveInput;

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
		//if (Input.GetButton("Jump") && !isGrounded)
		//{
		//	jumpInput = true;
		//	if (airHoverTime >= 0)
		//	{
		//		airHoverTime -= Time.deltaTime;
		//		upForce = airHoverForce;
		//	}
		//}
		if (Input.GetButtonUp("Jump"))
		{
			jumpInput = false;
		}


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
		rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		if (isGrounded)
		{
			airHoverTime = maxAirHoverTime;
		}

		if (jumpInput && airHoverTime >= 0)
		{
			rb.AddForce(Vector2.up * upForce);
			//rb.velocity = Vector2.up * upForce;
		}		
	}

	private void LookLeft()
	{
		transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 180, 0));
		gun.GetComponent<SpriteRenderer>().flipY = true;
	}

	private void LookRight()
	{
		transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
		gun.GetComponent<SpriteRenderer>().flipY = false;
	}
}
