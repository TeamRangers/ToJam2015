using UnityEngine;
using System.Collections;

public interface WeaponProperties {
	float Firerate{get;}
	Vector3 attack(GameObject reticle);
}

