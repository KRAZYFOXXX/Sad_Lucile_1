using UnityEngine;
using System.Collections;

public class CarBack : MonoBehaviour {
	
	public Animator anim;
	public AudioClip carBrake;
	public GameObject player;
	public Vector3 startPos;
	public bool descend;
	public int startOrder;
	public SpriteRenderer sprite;
	public bool beforeCollision;
	
	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer>();
		startOrder = sprite.sortingOrder;
		startPos = transform.position;
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		//la voiture avance
		if (player.GetComponent<Player> ().joue != true) {
			if(descend != true){
				transform.Translate (0, 3f * Time.deltaTime, 0);
			}else{
				transform.Translate (0, 0, -0.5f * Time.deltaTime);
			}
		}
		//si le joueur joue on arrete les animations des voitures pour que lucile puisse passer
		if (beforeCollision == true) {
			if (player.GetComponent<Player> ().joue == true) {
				anim.enabled = false;
			} else {
				anim.enabled = true;
			}
		}
	}
	
	void OnCollisionEnter2D (Collision2D col){
		//quand Lucile entre en contact avec la voiture la voiture freine
		if (col.gameObject.tag == "Player") {
			AudioSource.PlayClipAtPoint(carBrake, transform.position);
			player.GetComponent<Player> ().trebuche = true;
		}
	}
	
	void OnCollisionExit2D (Collision2D col){
		if (col.gameObject.tag == "Player") {
			player.GetComponent<Player> ().trebuche = false;		
		}
	}

	void Collision(){
		beforeCollision = true;
	}

	void AnimationStart(){
		transform.position = startPos;
		sprite.sortingOrder = startOrder;
		descend = false;		
		beforeCollision = true;
	}
	
	void Descend(){
		sprite.sortingOrder = 0;
		descend = true;
		beforeCollision = false;
	}
	
}
