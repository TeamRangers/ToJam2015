using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {
	public float speed;
	public int damage = 20;
	public int lifeTime = 2;

	public AudioClip[] destructionSounds;
	public AudioClip destSnd;

	AudioSource _audioSrc;
    
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
		_audioSrc = GetComponent<AudioSource>(); 
	}

	void OnCollisionEnter2D(Collision2D coll) {
		_audioSrc.PlayOneShot(destSnd);
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

	void PlayDestructionSound(){
		Debug.Log("Playing sound");
		if (destructionSounds.Length > 0){
			destructionSounds[0].LoadAudioData();
			_audioSrc.clip = destructionSounds[0];
			_audioSrc.PlayOneShot(destSnd);
			
		}
	}
}
