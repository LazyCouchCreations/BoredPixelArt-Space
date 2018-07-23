using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

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
	public int currentHealth;
	public int maxHealth;

	//attack
	public Transform[] losTargets;
	public int whichAttack;
	private Animator animator;

	//poddrick
	public GameObject peaPrefab;
	public Transform peaPos;
	public bool isThrowing;
	public float throwCooldown;
	private float remainingThrowCooldown;

	
	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		remainingThrowCooldown = 0;
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		losTargets = new Transform[3];
		losTargets[0] = player.transform;
		losTargets[1] = player.GetComponentInChildren<Transform>().Find("HeadPos");
		losTargets[2] = player.GetComponentInChildren<Transform>().Find("FeetPos");
		animator = GetComponent<Animator>();
		if (wayPoints[0] == null)
		{
			wayPoints[0] = player.transform;
		}
		currentWayPoint = 1;
	}
	
	// Update is called once per frame
	void Update () {
		LookForThePlayer(0);
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

		if (isGrounded && !isJumping && !isThrowing)
		{
			rb.velocity = new Vector2(mySpeed, rb.velocity.y);
		}

		RaycastHit2D rightWhiskerHit = Physics2D.Raycast(transform.position, transform.right, whiskerLength, whatIsGround);
		if (isGrounded && rightWhiskerHit)
		{
			rb.velocity = jumpRight.up * jumpForce;
		}
		
		if(remainingThrowCooldown > 0)
		{
			remainingThrowCooldown -= Time.fixedDeltaTime;
		}
		else
		{
			remainingThrowCooldown = 0;
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
			return 1;
		}
	}

	public void TakeDamage()
	{
		currentHealth -= 1;
		if(currentHealth <= 0)
		{
			Death();
		}
	}

	private void Death()
	{
		Destroy(gameObject);
	}

	private void LookForThePlayer(int targID)
	{
		try
		{
			lineOfSightPos.LookAt(losTargets[targID]);
			RaycastHit2D lineOfSightToPlayer;

			if (lineOfSightToPlayer = Physics2D.Raycast(lineOfSightPos.position, lineOfSightPos.forward, lineOfSightDistance, whatIsLineOfSight))
			{
				if (lineOfSightToPlayer.transform.tag == "Player")
				{
					switch (whichAttack)
					{
						case 0:
							Poddrick();
							break;
						case 1:
							Broccolist();
							break;
					}						
				}
			}
			else
			{
				LookForThePlayer(targID + 1);
			}
		}
		catch
		{
			//do nothing, ran out of targets
		}		
	}

	private void Broccolist()
	{
		
	}

	private void Poddrick()
	{
		if(remainingThrowCooldown == 0)
		{
			remainingThrowCooldown = throwCooldown;
			currentWayPoint = 0;
			animator.SetTrigger("Throw");
			isThrowing = true;
		}		
	}

	private void Poddrick_Throw()
	{
		Instantiate(peaPrefab, peaPos.position, lineOfSightPos.rotation);
	}

	private void Poddrick_StopThrowing()
	{
		isThrowing = false;		
		currentWayPoint = FindNextWaypoint(currentWayPoint);
	}

}
