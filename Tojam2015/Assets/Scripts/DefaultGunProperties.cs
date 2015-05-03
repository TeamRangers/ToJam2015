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

	public string Owner {
		get {
			return owner;
		}
		set {
			owner = value;
		}
	}

	private string owner;

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public Vector2 attack(GameObject reticle) {
		return shoot(reticle);
	}

	Vector2 shoot(GameObject reticle) {
		Vector2 target = reticle.transform.position; //Get reticle position
		Vector2 projectileOrigin = (Vector2)transform.position;
		
		Vector2 targetDir = (target - projectileOrigin).normalized;
		
		//Determine the rotation for the projectile we are about to spawn by using the vector from us to the reticle
		Quaternion projectileRotation = Quaternion.FromToRotation(Vector3.up, targetDir);
		
		//Create a new projectile, 1 unit away from us, facing the direction of the reticle
		GameObject projectile = Instantiate(projectileObject, Vector3.MoveTowards(projectileOrigin, target, 0.5f), projectileRotation) as GameObject; 
		ProjectileMover projectileMover = projectile.GetComponent<ProjectileMover>();
		projectileMover.setAttacker(Owner);
		projectileMover.Fire(targetDir);

		return -targetDir;
		//Add some recoil of a fixed magnitude
		//_rb2D.AddForce(-targetDir * recoilStrength, ForceMode2D.Impulse);
	}
}

