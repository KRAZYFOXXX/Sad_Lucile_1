using UnityEngine;
using System.Collections;

public class soundParent : MonoBehaviour {

	public bool dispute;
	public bool sound;

	public AudioClip sound_dispute;
	public AudioSource audio;
	public float audioLengthFight = 0;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((dispute == true) && (sound == true)){
			SDispute();
		}
	}

	void OnTriggerStay2D(Collider2D col)
	{
		dispute = true;
	}
	
	
	void OnTriggerExit2D(Collider2D col)
	{
		dispute = false;
	}
	
	//routine
	IEnumerator WaitForReload(float soundLength) { 
		sound = true; 
		yield return new WaitForSeconds(soundLength); 
		sound = false; 
	} 
	
	//Fct called quand on marche
	void SDispute() {
		//print ("in walk");
		audio.clip = sound_dispute;
		audio.volume = 2f;
		audio.Play ();
		
		StartCoroutine (WaitForReload (audioLengthFight));
	}
}
