using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class SceneManager : MonoBehaviour {

    // Singleton
    private static SceneManager _instance = null;
    public static SceneManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    public GameObject forceField;
    public GameObject gameOverPanel;
    public GameObject playerOneWinsText;
    public GameObject playerTwoWinsText;
    public GameObject mainMenuButton;
    public GameObject P2Reticle;

    public void GameOver(int winner)
    {
        gameOverPanel.SetActive(true);
        Cursor.visible = true;

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
        mainMenuButton.GetComponent<Button>().onClick.AddListener(ArenaManager.Instance.LoadMainMenu);
        forceField.GetComponent<ForceField>().Generate();

        // If 2 player turn off AI
        if (ArenaManager.Instance.NumberOfPlayers == 2)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponent<RobotPlayer>().activeAI = false;
                P2Reticle.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        
	}

}
