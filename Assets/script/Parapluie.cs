using UnityEngine;
using System.Collections;

public class Parapluie : MonoBehaviour {

	private Rigidbody2D rb2d;
	public bool kinematic;
	public GameObject player;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		rb2d = GetComponent<Rigidbody2D> ();
		kinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (kinematic == false) {
			rb2d.isKinematic = false;
		} else {
			rb2d.isKinematic = true;
		}
	}

	void OnCollisionEnter2D (Collision2D col){
		//quand Lucile entre en contact le parapluie, elle le prend (animation) et s'envole vers le niveau 2
		if (col.gameObject.tag == "Player") {
			player.GetComponent<Player> ().parapluieTaken = true;
			Destroy(gameObject);
		}
	}

}
