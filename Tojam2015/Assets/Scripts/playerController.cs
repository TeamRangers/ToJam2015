using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour {
	public GameObject projectileObject;

	// Use this for initialization
	public float speed; //Speed coefficient determines thruster strenght
	public float jumpSpeed; //Jump speed determines jump strenght


	//Names of the inputs defined in Edit->Project->Input
	public string Horizontal;
	public string Vertical;
	public string Fire;
	public string Jump;

	public float AttackSpeed; //Delay in seconds
	public GameObject reticle; //Reticle object used for aiming

	public bool activeAI; //Determines whether this player is controlled by AI

	private float nextAttackTime;
	private Rigidbody2D rb;

	private IList<GameObject> enemies;
	
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();

		if (activeAI){
			GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Player");
			enemies = new List<GameObject>();
			for (int i =0; i < enemiesArray.Length; i++){
				if (enemiesArray[i] != gameObject) {enemies.Add(enemiesArray[i]);}
			}
		}
	} 

	void FixedUpdate ()
	{
		if (activeAI){aiTick(); return;}
		float moveHorizontal = Input.GetAxis (Horizontal);
		float moveVertical = Input.GetAxis (Vertical);
		
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		
		rb.AddForce (movement * speed);
		//Vector2 v = rb.velocity;
		//v.x =  moveHorizontal * speed;
		//v.y = moveVertical * speed;
		//rb.velocity = v;
	}
	
	void Update ()
	{
		if (activeAI) {return;}
		bool down = Input.GetButtonDown(Jump);
		if (down) {
			rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
		}

		if (Input.GetButtonDown(Fire)){
			FireProjectile();

	}
}
	void FireProjectile(){
		if (Time.time > nextAttackTime){ //Check if we are allowed to perform an attack
		Vector3 target = reticle.transform.position; //Get reticle position
		target.z = 0;

		Quaternion projectileRotation = Quaternion.LookRotation(target - transform.position);
		Instantiate(projectileObject, Vector3.MoveTowards(transform.position, target, 1), projectileRotation);
		rb.AddForce((transform.position - target).normalized, ForceMode2D.Impulse);

		nextAttackTime = Time.time + aiAttackSpeed;
		}
	}

	void aiTick(){
		foreach (GameObject enemy in enemies) {
			if (Random.Range(0, 100) > 100/enemies.Count){continue;}
			if (enemy.activeSelf){
				reticle.transform.position = enemy.transform.position;
				FireProjectile();
				return;
			}
		}
	}
}