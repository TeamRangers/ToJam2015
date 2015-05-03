using UnityEngine;
using System.Collections;

public interface WeaponProperties {
	float Firerate{get;}
	string Owner{get;set;}
	Vector2 attack(GameObject reticle);
}

