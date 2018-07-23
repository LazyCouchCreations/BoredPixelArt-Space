using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startscene : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		player.transform.SetPositionAndRotation(transform.position, player.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
