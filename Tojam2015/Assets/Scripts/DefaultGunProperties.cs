using UnityEngine;
using System.Collections;

public class DefaultGunProperties : MonoBehaviour, WeaponProperties
{
	public float firerate = 0.3f;
	public GameObject projectileObject;
	public float Firerate {
		get {
			return firerate;
		}
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public Vector3 attack(GameObject reticle) {
		return shoot(reticle);
	}

	Vector3 shoot(GameObject reticle) {
		Vector3 target = reticle.transform.position; //Get reticle position
		target.z = 1;
		
		//Determine the rotation for the projectile we are about to spawn by using the vector from us to the reticle
		Quaternion projectileRotation = Quaternion.LookRotation(target - transform.position);
		
		//Create a new projectile, 1 unit away from us, facing the direction of the reticle
		Instantiate(projectileObject, Vector3.MoveTowards(transform.position, target, 2), projectileRotation);

		//Return recoil vector
		return (transform.position - target).normalized;

	}
}

