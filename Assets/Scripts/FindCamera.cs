using UnityEngine;

public class FindCamera : MonoBehaviour {

	private Canvas canvas;
	private GameObject cam;

	// Use this for initialization
	void Start () {
		canvas = GetComponent<Canvas>();
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		canvas.worldCamera = cam.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
