using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public GameObject audioObject, audioPrefab;

	// Use this for initialization
	void Awake () {
		if (GameObject.FindGameObjectWithTag("Audio") == false)
        {
            audioObject = Instantiate(audioPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
