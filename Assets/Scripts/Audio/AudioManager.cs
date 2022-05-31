using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}
    AudioSource audioPlayer;



    private void Awake() 
    { 
    // If there is an instance, and it's not me, delete myself.
    
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
            DontDestroyOnLoad(this.gameObject);
        } 
    }

    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        //playBGM();

    }

    public void playBGM()
    {
        audioPlayer.Play();
    }

    public void playSound( AudioClip sound)
    {
        audioPlayer.PlayOneShot(sound);
    }
}
