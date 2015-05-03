using UnityEngine;
using System.Collections;

public interface WeaponProperties {
	float Firerate{get;}
	string Owner{get;set;}
	float RecoilStrength{get;}
	Vector2 attack(GameObject reticle);
}

