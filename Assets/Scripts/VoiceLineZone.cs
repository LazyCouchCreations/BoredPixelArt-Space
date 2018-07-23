using UnityEngine;

public class VoiceLineZone : MonoBehaviour {

	private AudioSource audioSource;
	private AudioClip audioClip;
	private BoxCollider2D boxCollider2D;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		audioClip = audioSource.clip;
		boxCollider2D = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Player")
		{
			audioSource.PlayOneShot(audioClip);
			boxCollider2D.enabled = false;
		}
	}
}
