using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {

    public AudioClip[] clips;

    private int currentClip = -1;
    private AudioSource audioSource;

	// Use this for initialization
	void Awake () {
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (audioSource.isPlaying == false)
        {
            if (currentClip < clips.Length - 1)
            {
                currentClip += 1;
            } else { currentClip = 0; }
            audioSource.PlayOneShot(clips[currentClip]);
        }
	}

    public void Mute()
    {
        audioSource.mute = !audioSource.mute;
    }

    public bool GetSoundStatus()
    {
        return audioSource.mute;
    }
}
