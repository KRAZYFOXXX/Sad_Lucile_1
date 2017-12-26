using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour {

	public Transform[] waypoint;
	public float speed = 1;
	public bool loop = true;
	public float pauseDuration = 0;
	public float vitesseRotation = 5f;
	
	private float currentTime;
	private int currentWaypoint;

	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

		if (currentWaypoint < waypoint.Length) {
			Patrouille ();
		} else {
			if(loop){
				currentWaypoint = 0;
			}
		}
	}

	void Patrouille() {
		Vector3 cible = waypoint[currentWaypoint].position;
		cible.y = transform.position.y;
		Vector3 moveDirection = cible - transform.position;
		if (currentWaypoint == 1) {
			moveDirection.x = -moveDirection.x;
		}

		if(moveDirection.magnitude < 0.5){
			if(currentTime == 0){
				currentTime = Time.time; //pause au checkpoint
			}
			if(	(Time.time - currentTime) >= pauseDuration){
				if (currentWaypoint == 1) {
					transform.eulerAngles = new Vector2(0,0);
				}else{
					transform.eulerAngles = new Vector2(0,180);
				}
				currentWaypoint++;
				currentTime = 0;
			}
		}else{
			transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
		}
	}
}
