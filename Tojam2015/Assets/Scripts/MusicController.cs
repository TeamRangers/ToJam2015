using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour {

    // Singleton
    private static MusicController instance = null;
    public static MusicController Instance
    {
        get { return instance; }
    }

    AudioSource _audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;        
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }    

    public void Toggle()
    {
        _audioSource.mute = !_audioSource.mute;        
    }
}
