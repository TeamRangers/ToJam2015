﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotPlayer : MonoBehaviour {
    
    public float jumpForce;
    public float surfaceWalkSpeed;
    public string horizontalAxis;
    public string jumpButton;
	
	public string fire;
	public GameObject reticle;
	public GameObject projectileObject;

	public float recoilStrength;
	public float attackDelay;
	private float nextAttackTime;

	public bool activeAI;

	private IList<GameObject> enemies;
    public PlayerState playerState;

    Animator _animator;
    ForceField _forceField;

    public enum PlayerState
    {
        Floating,
        AboutToLand,
        OnSurface,
        LeavingSurface,
        Dead
    };

    PlayerState _state;
    SphereAttractor _surface;

    Rigidbody2D _rb2D;
    ConstantForce2D _cf2D;

	// Use this for initialization
	void Awake () {
        _rb2D = GetComponent<Rigidbody2D>();
        _cf2D = GetComponent<ConstantForce2D>();
        _state = PlayerState.Floating;
        _surface = null;
        _animator = GetComponent<Animator>();        
	}

    void Start()
    {
        _forceField = GameObject.FindGameObjectWithTag("ForceField").GetComponent<ForceField>();

		if (activeAI){ //Construct a list of enemies (everyone tagged Player except oneself)
			GameObject[] enemiesArray = GameObject.FindGameObjectsWithTag("Player");
			enemies = new List<GameObject>();
			for (int i =0; i < enemiesArray.Length; i++){
				if (enemiesArray[i] != gameObject) {enemies.Add(enemiesArray[i]);}
			}
		}
    }
	
	// Update is called once per frame
	void Update () {

        if (_state == PlayerState.Floating)
        {
            // Sample gravity from field
            _cf2D.force = _forceField.GetForce(transform.position);            
            _animator.SetBool("Floating", true);            
        }
        else if (_state == PlayerState.AboutToLand)
        {
            // Turn off physics and fix to surface
            _rb2D.isKinematic = true;
            _animator.SetBool("Floating", false);
            _cf2D.force = Vector2.zero;

            Vector2 surfaceNormal = transform.position - _surface.transform.position;            
            transform.position = (Vector3) surfaceNormal.normalized * _surface.radius + _surface.transform.position;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);

            _state = playerState = PlayerState.OnSurface;
        }
        else if (_state == PlayerState.OnSurface)
        {
            float hInput = Input.GetAxis(horizontalAxis);
            Vector2 surfacePosition = transform.position - _surface.transform.position;

            if (Mathf.Abs(hInput) > 0)
            {                
                // Move (rotate) around planet surface
                float theta = Mathf.Atan2(surfacePosition.y, surfacePosition.x);
                theta -= hInput * surfaceWalkSpeed * Time.deltaTime / _surface.radius;

                transform.position = _surface.transform.position + _surface.radius * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);                

                _animator.SetBool("Walking", true);
            }
            else
            {
                _animator.SetBool("Walking", false);
            }

            transform.rotation = Quaternion.FromToRotation(Vector3.up, surfacePosition.normalized);

            if (Input.GetButton(jumpButton) || activeAI)
            {   
				int aiRoll;
				if (activeAI) {aiRoll = Random.Range(1, 2);} else {aiRoll = 1;}
                // Give a nudge off the surface before turning physics back on
                transform.position = _surface.transform.position + (Vector3) surfacePosition * 1.01f;
                _animator.SetBool("Walking", false);
                _rb2D.isKinematic = false;
                _rb2D.AddForce(surfacePosition.normalized * jumpForce * aiRoll, ForceMode2D.Impulse);                
                _state = playerState = PlayerState.LeavingSurface;           
            }
        }
        else if (_state == PlayerState.LeavingSurface)
        {
            // Placeholder
            _state = playerState = PlayerState.Floating;            
        }


		//Handle firing here
		if (Input.GetButtonDown (fire)){
			FireProjectile();
		}

	}

    void OnCollisionEnter2D(Collision2D col2D)
    {
        if (col2D.gameObject.tag == "SphereAttractor" && _state == PlayerState.Floating)
        {
            _state = playerState = PlayerState.AboutToLand;
            _surface = col2D.gameObject.GetComponent<SphereAttractor>();
        }
    }


	void FireProjectile(){
		if (Time.time > nextAttackTime){ //Check if we are allowed to perform an attack
			Vector3 target = reticle.transform.position; //Get reticle position
			target.z = 1;
			
			//Determine the rotation for the projectile we are about to spawn by using the vector from us to the reticle
			Quaternion projectileRotation = Quaternion.LookRotation(target - transform.position);

			//Create a new projectile, 1 unit away from us, facing the direction of the reticle
			Instantiate(projectileObject, Vector3.MoveTowards(transform.position, target, 2), projectileRotation);
			
			//Add some recoil of a fixed magnitude
			_rb2D.AddForce((transform.position - target).normalized * recoilStrength, ForceMode2D.Impulse);
			
			nextAttackTime = Time.time + attackDelay; //Set the next attack time to be current time + delay
		}
	}
}
