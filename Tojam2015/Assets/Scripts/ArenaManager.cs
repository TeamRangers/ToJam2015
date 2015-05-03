using UnityEngine;
using System.Collections;

public class ArenaManager : MonoBehaviour {

    // Singleton
    private static ArenaManager _instance = null;
    public static ArenaManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ArenaManager>();
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

	void Update(){
		if (Input.GetKeyDown("escape")){
			Cursor.visible = true;
			Application.LoadLevel("MainMenu");
		}
	}

    int _numberOfPlayers = 1;

    public int NumberOfPlayers
    {
        get { return _numberOfPlayers; }
    }

    public void LoadOnePlayer()
    {
        _numberOfPlayers = 1;
        Application.LoadLevel("MainArena");
    }

    public void LoadTwoPlayer()
    {
        _numberOfPlayers = 2;
        Application.LoadLevel("MainArena");
    }
}
