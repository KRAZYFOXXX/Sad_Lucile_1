using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public Animator anim;
	public float speed = 4f;
	public float jumpPower = 8000f;

	//variable avec animation
	public bool echelle;
	public bool useDino;
	public bool interagir;
	public bool joue;
	public bool invincible;
	public bool embrasser;
	public bool trebuche;

	public AudioClip rire;

	//gestion du vol
	public bool parapluieTaken;
	public bool envol;
	public Transform[] waypoint;
	public float envolSpeed = 2f;
	public bool loop = false;
	public float pauseDuration = 0;
	private float currentTime;
	private int currentWaypoint;
	public AudioClip vent;

	public Rigidbody2D rb2d;

	//gestion du sol
	public bool grounded;
	public bool onRoad;
	public bool onStreet;
	public float groundCheckRadius;
	public Transform groundCheck;
	public LayerMask whatIsRoad;
	public LayerMask whatIsGround;
	public LayerMask whatIsStreet;

	//Bruits de pas
	public AudioClip[] footStep;
	public AudioSource audio;
	private bool step = true; 
	private float audioStepLengthWalk = 0.55f; 

	//public AudioClip walkSound;
	
	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource>();
	}

	void FixedUpdate () {
		//détection du sol
		if (Application.loadedLevel == 2) {
			if(echelle == false){
				grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
			}
		} else {
			onRoad = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsRoad);
			onStreet = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsStreet);
			if ((onRoad == true) || (onStreet == true)) {
				grounded = true;
			} else {
				grounded = false;
			}
		}
	}
	
	// Update is called once per frame 
	void Update () {
		invincible = false;
		//float h = Input.GetAxis ("Vertical");
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");
		
		anim.SetBool ("grounded", grounded);

		//dino
		if (Input.GetKeyDown (KeyCode.R)) {
			AudioSource.PlayClipAtPoint(rire, transform.position);
			useDino = true;
		}
		if (Input.GetKeyUp (KeyCode.R)) {
			useDino = false;
		}

		//interagir
		if (Input.GetKeyDown (KeyCode.E)) {
			interagir = true;
		}
		if (Input.GetKeyUp (KeyCode.E)) {
			interagir = false;
		}

		if ((useDino == true) || (joue == true)) {
			invincible = true;
		}

		anim.SetBool ("useDino", useDino);
		//empeche le mouvement sur utilisation du dino
		//echelle
		Physics2D.IgnoreLayerCollision (8, 17, false); //mur
		Physics2D.IgnoreLayerCollision (8, 11, false); //sol

		if(echelle == true){
			grounded = true;
			Physics2D.IgnoreLayerCollision (8, 17, true); //mur
			Physics2D.IgnoreLayerCollision (8, 11, true); //sol
			if(Input.GetAxis("Vertical") != 0){
				transform.Translate (0, y * speed * Time.deltaTime, 0);
			}
		}else if ((useDino == false) && (embrasser == false) && (envol == false)) { 
			anim.SetFloat ("speed", Mathf.Abs (x));

			//saut
			if (Input.GetKeyDown (KeyCode.Space) && grounded == true) {
				rb2d.AddForce (Vector2.up * jumpPower);	
			}
			
			//anim.SetBool ("trebuche", trebuche); //à corriger plus tard
			//navigation gauche/droite
			if (trebuche == true) {
				//on fait trebucher lucile, elle recule en tombant
				if (x > 0) {
					transform.Translate (-1, 0, 0);
				}
				if (x < 0) {
					transform.Translate (1, 0, 0);
				}
				trebuche = false;
			} else if (trebuche == false) {
				if (x > 0) {
					transform.Translate (x * speed * Time.deltaTime, 0, 0);
					transform.eulerAngles = new Vector2 (0, 0);
				}
				if (x < 0) {
					transform.Translate (-x * speed * Time.deltaTime, 0, 0);
					transform.eulerAngles = new Vector2 (0, 180);
				}
			}

			/*if (grounded && rb2d.velocity.magnitude > 0f && step == true) {
				WalkOnRoad ();
			} */
		}
		
		if (envol == true) {
			if (currentWaypoint < waypoint.Length) {
				Envol ();
			} else {
				if(loop){
					currentWaypoint = 0;
				}
			}
		}

		//on ignore les collisions avec les ennemis si on utilise le dino
		if (useDino == true) {
			Physics2D.IgnoreLayerCollision (8, 16, true); 
		} else {
			Physics2D.IgnoreLayerCollision (8, 16, false); 
		}

		//on ignore les collions ballon/voiture pour que la balle puisse aller sur la route directement
		Physics2D.IgnoreLayerCollision (10, 9, true);

		//on ignore les collisions si le joueur est invincible: avec la voiture
		Physics2D.IgnoreLayerCollision (8, 9, invincible);
		//on ignore les collisions avec le ballon si on n'interagit pas
		if (interagir == false){
			Physics2D.IgnoreLayerCollision (8, 10, true); //ballon
			Physics2D.IgnoreLayerCollision (8, 14, true); //couple
			Physics2D.IgnoreLayerCollision (8, 15, true); //parapluie
		} else if (interagir == true){
			Physics2D.IgnoreLayerCollision (8, 10, false); //ballon
			Physics2D.IgnoreLayerCollision (8, 14, false); //couple
			Physics2D.IgnoreLayerCollision (8, 15, false); //parapluie
		}

		anim.SetBool ("echelle", echelle);
		anim.SetBool ("envol", envol);
		anim.SetBool ("embrasser", embrasser);
		anim.SetBool ("parapluieTaken", parapluieTaken);
	}

	void Envol() {
		rb2d.gravityScale = 0;
		Vector3 cible = waypoint[currentWaypoint].position;
		//cible.y = transform.position.y;
		Vector3 moveDirection = cible - transform.position;
		
		if(moveDirection.magnitude < 0.5){
			if(currentTime == 0){
				currentTime = Time.time; //pause au checkpoint
			}
			if(	(Time.time - currentTime) >= pauseDuration){
				if(currentWaypoint == (waypoint.Length - 1)){
					Application.LoadLevel(Application.loadedLevel + 1);
				}
				currentWaypoint++;
				currentTime = 0;
			}
		}else{
			transform.Translate(moveDirection.normalized * speed * Time.deltaTime);
		}
	}

	void ParapluieTaken() {
		envol = true;

		audio.clip = vent;
		audio.volume = 2f;
		audio.Play ();
	}

	//routine
	IEnumerator WaitForFootSteps(float stepsLength) { 
		step = false; 
		yield return new WaitForSeconds(stepsLength); 
		step = true; 
	} 
	
	//Fct called quand on marche
	void WalkOnRoad() {
		audio.clip = footStep[Random.Range(0,footStep.Length-1)];	
		
		audio.volume = 0.4f;
		audio.Play ();
	}
}
