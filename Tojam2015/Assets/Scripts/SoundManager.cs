using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	AudioSource _audioSrc;

	// Use this for initialization
	void Start () {
		_audioSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlaySound(AudioClip sound){
		_audioSrc.PlayOneShot(sound);
	}
}
