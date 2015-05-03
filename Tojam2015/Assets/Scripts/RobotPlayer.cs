using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RobotPlayer : MonoBehaviour {
    
    public float jumpForce;
    public float surfaceWalkSpeed;
    public string horizontalAxis;
    public string jumpButton;
	
	public string fire;
	public GameObject reticle;
	//public GameObject projectileObject;
	public GameObject weaponObject;

	public float recoilStrength;
	public float attackDelay;
	//private float nextAttackTime;

	private float attackTimer;
	public bool activeAI;

	private IList<GameObject> enemies;
    public PlayerState playerState;

	private bool isWalking; //This is a bool for AI to see whether he is walking;
	private GameObject targetEnemy;
	private float maxWalkTime = 0.75f;

	//Sounds
	private SoundManager soundManager;
	public AudioClip jumpLandingSound;

	WeaponProperties weaponProperties;
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

    public void Die()
    {
        _state = PlayerState.Dead;
        _animator.SetBool("Dead", true);
        _rb2D.GetComponent<Collider2D>().enabled = false;
    }

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
		weaponProperties = (WeaponProperties)weaponObject.GetComponent(typeof(WeaponProperties));
        _forceField = GameObject.FindGameObjectWithTag("ForceField").GetComponent<ForceField>();
		soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

		if (activeAI){ //Construct a list of enemies (everyone tagged Player except oneself)
			enemies = new List<GameObject>();
			for (int i = 1; i < 5; i++){
				GameObject enemy = GameObject.FindGameObjectWithTag("Player" + i.ToString());
				if (enemy != null && enemy != gameObject) {enemies.Add(enemy);}
			}

			//Disable reticle renderer since AI doesn't need it
			reticle.GetComponent<SpriteRenderer>().enabled = false;
		}
    }
	
	// Update is called once per frame
	void Update () {

		attackTimer += Time.deltaTime;

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

			soundManager.PlaySound(jumpLandingSound);

            _state = playerState = PlayerState.OnSurface;
        }
        else if (_state == PlayerState.OnSurface)
        {
            float hInput = Input.GetAxis(horizontalAxis);
            Vector2 surfacePosition = transform.position - _surface.transform.position;

            if (Mathf.Abs(hInput) > 0 || (activeAI && isWalking))
            {                
                // Move (rotate) around planet surface
				float theta = Mathf.Atan2(surfacePosition.y, surfacePosition.x);

				if (activeAI) {

					Vector2 playerNormal = (targetEnemy.transform.position - _surface.transform.position).normalized;
					Vector2 aiNormal = surfacePosition.normalized;
					
                    float crossMag = aiNormal.x * playerNormal.y - aiNormal.y * playerNormal.x;
                    float playerAngle = Mathf.Atan2(crossMag, Vector2.Dot(aiNormal, playerNormal));

					if (Mathf.Abs(playerAngle) < Mathf.PI * 0.05f) { isWalking = false; }                    

					if (playerAngle < 0)
                    {
                        // Player is to the left
						theta -= surfaceWalkSpeed * Time.deltaTime / _surface.radius;
					}
                    else 
					{
						theta += surfaceWalkSpeed * Time.deltaTime / _surface.radius;
					}
				}
				else
				{
                    theta -= hInput * surfaceWalkSpeed * Time.deltaTime / _surface.radius;
				}

                transform.position = _surface.transform.position + _surface.radius * new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);                

                _animator.SetBool("Walking", true);
            }
            else
            {
                _animator.SetBool("Walking", false);
            }

            transform.rotation = Quaternion.FromToRotation(Vector3.up, surfacePosition.normalized);

            if (Input.GetButton(jumpButton) || (activeAI && !isWalking))
            {   
				int aiRoll;
				if (activeAI) {aiRoll = Random.Range(1, 2);
				if (Random.Range(0, 100) > 20){
						foreach (GameObject enemy in enemies) {
							if (Random.Range(0, 100) > 100/enemies.Count){continue;}
							if (enemy.activeSelf){
								isWalking = true;
								targetEnemy = enemy;
								break;
							}}
					}
				} else {aiRoll = 1;}

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
        if (_state != PlayerState.Dead)
        {
		if (activeAI){AiTick(); return;}
		//Handle firing here
		if (Input.GetButtonDown (fire)){
			AttackUsingWeapon();
		}
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


	void AttackUsingWeapon(){
		if (attackTimer >= weaponProperties.Firerate){ //Check if we are allowed to perform an attack
			attackTimer = 0f;

			Vector3 recoilVector = weaponProperties.attack (reticle);

			//Add some recoil of a fixed magnitude
			_rb2D.AddForce(recoilVector * recoilStrength, ForceMode2D.Impulse);
	// void FireProjectile ()
    // {
		// if (Time.time > nextAttackTime){ //Check if we are allowed to perform an attack
			// Vector2 target = reticle.transform.position; //Get reticle position
			// Vector2 projectileOrigin = (Vector2)transform.position +(Vector2)transform.up.normalized * 0.6f;

            // Vector2 targetDir = (target - projectileOrigin).normalized;

			//Determine the rotation for the projectile we are about to spawn by using the vector from us to the reticle
			// Quaternion projectileRotation = Quaternion.FromToRotation(Vector3.up, targetDir);

			//Create a new projectile, 1 unit away from us, facing the direction of the reticle
			// GameObject projectile = Instantiate(projectileObject, Vector3.MoveTowards(projectileOrigin, target, 0.5f), projectileRotation) as GameObject;            
            // projectile.GetComponent<ProjectileMover>().Fire(targetDir);
			
			//Add some recoil of a fixed magnitude
			// _rb2D.AddForce(-targetDir * recoilStrength, ForceMode2D.Impulse);
			
			//nextAttackTime = Time.time + attackDelay; //Set the next attack time to be current time + delay
		}
	}

	void AiTick(){
		foreach (GameObject enemy in enemies) {
			if (Random.Range(0, 100) > 100/enemies.Count){continue;}
			if (enemy.activeSelf){
				reticle.transform.position = enemy.transform.position;
				AttackUsingWeapon();
				return;
			}
		}
	}
}
