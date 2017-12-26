using UnityEngine;
using System.Collections;

public class Echelle : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			player.GetComponent<Player> ().transform.Translate(0.55f,-1f,0);
			player.GetComponent<Player> ().transform.eulerAngles = new Vector2 (0, 180);
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			player.GetComponent<Player> ().echelle = true;
			player.GetComponent<Player> ().rb2d.gravityScale = 0;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") {
			player.GetComponent<Player> ().echelle = false;
			player.GetComponent<Player> ().rb2d.gravityScale = 1;
		}	
	}
}
