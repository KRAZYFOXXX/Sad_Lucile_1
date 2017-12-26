using UnityEngine;
using System.Collections;

public class SkyScrolling : MonoBehaviour {

	public float speed_skyScroll = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 offset = new Vector2 (Time.time * speed_skyScroll, 0);
		GetComponent<Renderer>().material.mainTextureOffset = offset; 
	}
}
