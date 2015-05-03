using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    // Singleton
    private static SceneManager _instance = null;
    public static SceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SceneManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(this.gameObject);
        }
    }

    public GameObject forceField;
    public GameObject gameOverPanel;
    public GameObject playerOneWinsText;
    public GameObject playerTwoWinsText;

    public void GameOver(int winner)
    {
        gameOverPanel.SetActive(true);

        if (winner == 1)
        {
            playerOneWinsText.SetActive(true);
            playerTwoWinsText.SetActive(false);
        }
        else if (winner == 2)
        {
            playerOneWinsText.SetActive(false);
            playerTwoWinsText.SetActive(true);
        }
    }

	// Use this for initialization
	void Start () {

        gameOverPanel.SetActive(false);
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
