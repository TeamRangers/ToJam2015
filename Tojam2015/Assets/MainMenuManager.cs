using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.FindGameObjectWithTag("OnePlayerButton").GetComponent<Button>().onClick.AddListener(ArenaManager.Instance.LoadOnePlayer);
		GameObject.FindGameObjectWithTag("TwoPlayerButton").GetComponent<Button>().onClick.AddListener(ArenaManager.Instance.LoadTwoPlayer);
		//GameObject.FindGameObjectWithTag("MuteButton").GetComponent<Button>().onClick.AddListener(ArenaManager.Instance.);


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
