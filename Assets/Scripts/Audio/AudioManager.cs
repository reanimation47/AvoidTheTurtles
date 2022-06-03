using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}
    AudioSource audioPlayer;
    public AudioClip BGM;


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
        playBGM();

    }

    public void playBGM()
    {
        playSound(BGM,0.1f);
    }

    public void playSound( AudioClip sound, float volume)
    {
        audioPlayer.PlayOneShot(sound, volume);
    }
}
