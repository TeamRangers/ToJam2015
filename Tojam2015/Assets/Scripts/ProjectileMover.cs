using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
	public float projectileForce;
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
       
        _forceField = GameObject.FindGameObjectWithTag("ForceField").GetComponent<ForceField>();
	}

	public void Fire(Vector2 direction)
	{
		_rb2D.AddForce (direction.normalized * projectileForce, ForceMode2D.Impulse);
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
