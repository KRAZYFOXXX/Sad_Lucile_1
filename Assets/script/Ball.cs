using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public GameObject player;
	public bool onRoad;
	public bool onStreet;
	public AudioClip carBrake;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		//le joueur joue quand le ballon est sur la route
		if (onStreet == true) {
			player.GetComponent<Player> ().joue = false;
		} 
		if (onRoad == true) {
			player.GetComponent<Player> ().joue = true;
		} 
	}

	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.tag == "Road") {
			onRoad = true;
			//bruit de frein des voitures
			AudioSource.PlayClipAtPoint(carBrake, transform.position); 
		}
		if (col.gameObject.tag == "Street") {
			onStreet = true;
		} 
	}

	void OnCollisionStay2D (Collision2D col){
		if (col.gameObject.tag == "Road") {
			onRoad = true;
		}
		if (col.gameObject.tag == "Street") {
			onStreet = true;
		} 
	}
	
	void OnCollisionExit2D (Collision2D col){
		if (col.gameObject.tag == "Road") {
			onRoad = false;
		}
		if (col.gameObject.tag == "Street") {
			onStreet = false;
		}
	}
}
