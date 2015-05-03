using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
	public float speed;
	public int damage = 20;
	public int lifeTime = 2;
    
    ForceField _forceField;
    ConstantForce2D _cf2D;
    Rigidbody2D _rb2D;

    void Awake()
    {
        _cf2D = GetComponent<ConstantForce2D>();
        _rb2D = GetComponent<Rigidbody2D>();
		Destroy (gameObject, lifeTime);
    }

	// Use this for initialization
	void Start () {
        _rb2D.velocity = transform.forward * speed;
        _forceField = GameObject.FindGameObjectWithTag("ForceField").GetComponent<ForceField>();
	}

	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag.Contains("Player")) {

			PlayerHealth playerHealth = coll.gameObject.GetComponent<PlayerHealth>();

			if (playerHealth.currentHealth > 0) {
				playerHealth.TakeDamage(damage);
			}

		}

		Destroy(gameObject);
	}

	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate()
    {
        _cf2D.force = _forceField.GetForce(transform.position);
	}
}
