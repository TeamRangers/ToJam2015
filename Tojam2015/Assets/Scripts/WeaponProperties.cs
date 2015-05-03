using UnityEngine;
using System.Collections;

public interface WeaponProperties {
	float Firerate{get;}
	Vector2 attack(GameObject reticle);
}

