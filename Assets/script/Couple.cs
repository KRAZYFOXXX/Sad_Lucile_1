using UnityEngine;
using System.Collections;

public class Couple : MonoBehaviour {

	public Animator anim;
	public GameObject player;
	public GameObject parapluie;
	public bool disparait;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		parapluie = GameObject.FindGameObjectWithTag("Parapluie");
		anim = GetComponent<Animator> ();
		disparait = false;
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("disparait", disparait);
	}

	void OnCollisionEnter2D (Collision2D col){
		//quand Lucile entre en contact avec la voiture la voiture freine
		if (col.gameObject.tag == "Player") {
			player.GetComponent<Player> ().embrasser = true;
			disparait = true;
		}
	}

	/*//routine
	IEnumerator WaitForRelacher(float hugLength) { 
		player.GetComponent<Player> ().embrasser = true; 
		yield return new WaitForSeconds(hugLength); 
		player.GetComponent<Player> ().embrasser = false; 
	}*/ 

	void Disparition (){
		parapluie.GetComponent<Parapluie> ().kinematic = false;		
		//StartCoroutine (WaitForRelacher (5));
		player.GetComponent<Player> ().embrasser = false; 
		Destroy (gameObject);
	}
}
