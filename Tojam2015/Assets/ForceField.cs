using UnityEngine;
using System.Collections;

public class ForceField : MonoBehaviour {

    public bool showVectors = false;
    public float fieldWidth;
    public float fieldHeight;
    public int fieldXDivisions;
    public int fieldYDivisions;
    public float forceConstant = 1.0f;

    RegularGrid _forces;

    public void Generate()
    {
        GameObject[] goAttractors = GameObject.FindGameObjectsWithTag("SphereAttractor");

        foreach (Vector2 point in _forces.GetPoints())
        {
            Vector2 force = Vector2.zero;

            foreach (GameObject goAttractor in goAttractors)
            {
                Vector2 distance = (Vector2) goAttractor.transform.position - point - (Vector2) transform.position;
                SphereAttractor attractor = goAttractor.GetComponent<SphereAttractor>();

                if (distance.magnitude <= attractor.radius)
                {
                    force = Vector2.zero;
                    break;
                }

                force += forceConstant * attractor.forceScale / distance.sqrMagnitude * distance.normalized;
            }

            _forces.SetValue(point, force);
        }
    }

    public Vector2 GetForce(Vector3 position)
    {
        return _forces.GetValue(position - transform.position);
    }

	// Use this for initialization
	void Awake () {
        _forces = new RegularGrid(fieldWidth, fieldHeight, fieldXDivisions, fieldYDivisions);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Vector2 origin = new Vector3(fieldWidth / 2.0f, fieldHeight / 2.0f, 0) + transform.position;
        Vector2 basis = new Vector2(fieldWidth / (float)fieldXDivisions, fieldHeight / (float)fieldYDivisions);

        // Draw bounds
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(origin, new Vector3(fieldWidth, fieldHeight, 0));

        // Draw points
        for (int y = 0; y < fieldYDivisions + 1; ++y)
        {
            for (int x = 0; x < fieldXDivisions + 1; ++x)
            {
                Vector3 position = new Vector3(basis.x * x, basis.y * y, 0) + transform.position;

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(position, 0.01f);

                if (showVectors && _forces != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(position, position + (Vector3) _forces.GetValue((Vector2) (position - transform.position)) / forceConstant);
                }
            }
        }

    }
}
