using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
	public float projectileForce;
	public int damage = 20;
	public int lifeTime = 2;

	public AudioClip[] destructionSounds;
	private SoundManager soundManager;
	
    
    ForceField _forceField;
    ConstantForce2D _cf2D;
    Rigidbody2D _rb2D;

	private string attacker;

    void Awake()
    {
        _cf2D = GetComponent<ConstantForce2D>();
        _rb2D = GetComponent<Rigidbody2D>();
		Destroy (gameObject, lifeTime);
    }

	// Use this for initialization
	void Start () {
       
        _forceField = GameObject.FindGameObjectWithTag("ForceField").GetComponent<ForceField>(); 
		soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
	}

	public void setAttacker(string name) {
		attacker = name;
	}

	public void Fire(Vector2 direction)
	{
		_rb2D.AddForce (direction.normalized * projectileForce, ForceMode2D.Impulse); 
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (!coll.gameObject.name.Equals(attacker)) {
			if (coll.gameObject.CompareTag("Player")) {

				PlayerHealth playerHealth = coll.gameObject.GetComponent<PlayerHealth>();

				if (playerHealth.currentHealth > 0) {
					playerHealth.TakeDamage(damage);
				}

			}
			soundManager.PlaySound(destructionSounds[Random.Range(0, destructionSounds.Length - 1)], 0.5f);
			Destroy(gameObject);
		}
	}


	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate()
    {
        _cf2D.force = _forceField.GetForce(transform.position);
	}
}
