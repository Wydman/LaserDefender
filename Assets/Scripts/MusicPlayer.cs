using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

    bool mute = false;

    static MusicPlayer instance = null;
    private void Awake()
    {
        // le music player est détruit dans le awake pour empecher une mili-seconde
        // de son le temps que le script passe a la méthode Start
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

	
    public void Mute()
    {
        if(mute == false)
        {
            Debug.Log("mute");
            GetComponent<AudioSource>().Pause();
            mute = true;
        }
        else 
        {
            GetComponent<AudioSource>().UnPause();
            Debug.Log("unmute");
            mute = false;
        }
    }

   


	
	void Update () {
		
	}
}
