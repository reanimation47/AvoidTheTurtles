using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance {get; private set;}

    [SerializeField] private GameObject FrontSection;
    [SerializeField] private GameObject LeaderboardSection;
    [SerializeField] private Text LeaderboardSection_Highscore;
    [SerializeField] private Button StartGame;
    [SerializeField] private Button Leaderboard;
    [SerializeField] private Button BackToMenu;

    //int[] arr = new int[] {1, 9, 6, 7, 5, 9};

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
        } 
    }

    void Start()
    {
        StartGame.onClick.AddListener( ()=> 
        {
            SceneManager.LoadScene("SampleScene");
        } );

        Leaderboard.onClick.AddListener( ()=>
        {
            LeaderboardSection_Highscore.text ="High Score:" + PlayerPrefs.GetInt("FirstHighScore",10).ToString();
            FrontSection.SetActive(false);
            LeaderboardSection.SetActive(true);
        } );

        BackToMenu.onClick.AddListener( ()=>
        {
            FrontSection.SetActive(true);
            LeaderboardSection.SetActive(false);
        } );
        //Array.Sort(arr);
        //Array.Reverse(arr);
        if(!PlayerPrefs.HasKey("FirstHighScore"))
        {
            PlayerPrefs.SetInt("FirstHighScore",0);
        }
        //PlayerPrefs.SetInt("FirstHighScore",0);
        //LeaderboardSection_Highscore.text = PlayerPrefs.GetInt("FirstHighScore",0).ToString();

        //AudioManager.instance.playBGM();
    }

    public int currentHighscore()
    {
        return PlayerPrefs.GetInt("FirstHighScore");
    }

    public bool checkHighScore(int score)
    {
        int currentHighscore = PlayerPrefs.GetInt("FirstHighScore");
        if(score > currentHighscore)
        {
            PlayerPrefs.SetInt("FirstHighScore",score);
            return true;
        }else
        {
            return false;
        }
    }

}
