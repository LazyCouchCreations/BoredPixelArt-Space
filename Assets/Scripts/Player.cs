using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

	private Rigidbody2D rb;
	public Transform startPos;
	public Transform middlePos;

	//running
	public float horSpeed;
	public float vertSpeed;
	private float moveInput;
	public float whiskerLength;
	public Transform leftWhisker;
	private RaycastHit2D leftWhiskerHit;
	public Transform rightWhisker;
	private RaycastHit2D rightWhiskerHit;

	//jumping
	public Transform feetPos;
	private bool jumpInput;
	public float checkRadius;
	public bool isGrounded;
	public bool isJumping;
	public LayerMask whatIsGround;
	public float groundJumpForce;
	public float airHoverForce;
	public float upForce;
	public float maxAirHoverTime;
	public float airHoverTime;
	public bool isFalling;

	//facing
	public bool isLookingLeft;
	public GameObject gun;
	public GameObject body;

	//animations
	public Animator animator;

	private static bool isCreated = false;

	private void Awake()
	{
		if (!isCreated)
		{
			DontDestroyOnLoad(gameObject);
			isCreated = true;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		
		moveInput = Input.GetAxisRaw("Horizontal");
		if(moveInput == 0)
		{
			animator.SetBool("isWalking", false);
		}
		else
		{
			animator.SetBool("isWalking", true);
			animator.SetFloat("walkSpeed", moveInput);
		}

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			jumpInput = true;
			upForce = groundJumpForce;
		}

		if (Input.GetButton("Jump") && isFalling)
		{
			jumpInput = true;
			upForce = airHoverForce;
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
		//detect slopes with whiskers
		leftWhiskerHit = Physics2D.Raycast(leftWhisker.position, leftWhisker.up, whiskerLength, whatIsGround);
		Debug.DrawRay(leftWhisker.position, leftWhisker.up * whiskerLength, Color.magenta);
		rightWhiskerHit = Physics2D.Raycast(rightWhisker.position, rightWhisker.right, whiskerLength, whatIsGround);
		Debug.DrawRay(rightWhisker.position, rightWhisker.right * whiskerLength, Color.magenta);

		if (rightWhiskerHit)
		{
			rb.velocity = rightWhisker.up * moveInput * horSpeed;
			isFalling = false;
		}else if (leftWhiskerHit)
		{
			rb.velocity = leftWhisker.right * -moveInput * horSpeed;
			isFalling = false;
		}
		else
		{
			rb.velocity = new Vector2(moveInput * horSpeed, rb.velocity.y);
		}

		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		if (isGrounded)
		{
			airHoverTime = maxAirHoverTime;
			isFalling = false;
		}

		if(rb.velocity.y < 0 && !rightWhiskerHit && !leftWhiskerHit && !isGrounded)
		{
			isFalling = true;
		}

		if (jumpInput && airHoverTime >= 0)
		{
			Vector2 halfwayVector = (new Vector2(rb.velocity.x, 0) + Vector2.up * upForce);
			rb.velocity = halfwayVector;
			jumpInput = false;
			isJumping = true;
			airHoverTime -= Time.fixedDeltaTime;
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

	public void Death()
	{
		startPos = GameObject.FindGameObjectWithTag("Start").transform;
		middlePos = GameObject.FindGameObjectWithTag("Middle").transform;
		if (transform.position.x > middlePos.position.x)
		{
			transform.position = middlePos.position;
		}
		else
		{
			transform.position = startPos.position;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.transform.tag == "Enemy")
		{
			Death();
		}
	}
}
