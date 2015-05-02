using UnityEngine;
using System.Collections;

public class SphereAttractor : MonoBehaviour {

    public float forceScale;
    public float radius;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
