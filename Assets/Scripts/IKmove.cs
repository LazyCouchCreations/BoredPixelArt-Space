using UnityEngine;

public class IKmove : MonoBehaviour {

	public Transform leftFacingIktarget;
	public Transform rightFacingIktarget;
	public GameObject player;
	public bool isLookingLeft;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isLookingLeft)
		{
			transform.position = leftFacingIktarget.position;
		}
		else
		{
			transform.position = rightFacingIktarget.position;
		}
	}

	private void Update()
	{
		isLookingLeft = player.GetComponent<Player>().isLookingLeft;
	}
}
