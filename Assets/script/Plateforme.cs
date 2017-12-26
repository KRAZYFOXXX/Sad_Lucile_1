using UnityEngine;
using System.Collections;

public class Plateforme : MonoBehaviour {

	public EdgeCollider2D plateforme;
	public bool oneWay = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	// Update is called once per frame
	void Update () 
	{
		plateforme.enabled = !oneWay; 
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		oneWay = true;
	}
	
	
	void OnTriggerExit2D(Collider2D other)
	{
		oneWay = false;
	}
}
