using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public GameObject forceField;

	// Use this for initialization
	void Start () {
        forceField.GetComponent<ForceField>().Generate();

        // If 2 player turn off AI
        if (ArenaManager.Instance.NumberOfPlayers == 2)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponent<RobotPlayer>().activeAI = false;
            }
        }

	}

}
