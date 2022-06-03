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

    //FontGUI
    [SerializeField] private GameObject frontGUI;
    public bool frontGUIisHidden = false;

    //LeaderboardGUI
    [SerializeField] private GameObject LeaderboardGUI;
    public bool LeaderboardGUIisHidden = true;

    //ShopGUI
    [SerializeField] private GameObject ShopGUI;

    [SerializeField] private GameObject clickEffect;

    // Vector3 sectionHidden = new Vector3(0,0,0);
    // Vector3 sectionShown = new Vector3(1,1,1);
    Vector3 frontGUIscaler = new Vector3(1,1,1);

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
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void DestroyMyself()
    {
        Destroy(gameObject);
        instance = null;
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

    void Update()
    {

        //Showing and Hiding frontGUI
        if(!frontGUI){return;}
        frontGUI.transform.localScale = frontGUIscaler;
        
    }

    void FixedUpdate()
    {
        if(frontGUIisHidden)
        {
            frontGUIscaler.x = Mathf.Lerp(frontGUIscaler.x, 0, 0.1f);
            frontGUIscaler.y = Mathf.Lerp(frontGUIscaler.y, 0, 0.1f);
        }else
        {
            frontGUIscaler.x = Mathf.Lerp(frontGUIscaler.x, 1, 0.1f);
            frontGUIscaler.y = Mathf.Lerp(frontGUIscaler.y, 1, 0.1f);
        }
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

    public void onClickEffect(Vector3 clickPosition)
    {

        clickEffect.transform.position = clickPosition;
        clickEffect.SetActive(false);
        clickEffect.SetActive(true);

    }

    //FrontUI
    public void toggleFrontGUI()
    {
        frontGUIisHidden = frontGUIisHidden ? false : true ;
    }

    public void toggleFrontGUIafter(float time)
    {
        StartCoroutine(toggleFrontGUIafterIE(time));
    }

    IEnumerator toggleFrontGUIafterIE(float time)
    {
        yield return new WaitForSeconds(time);
        toggleFrontGUI();
    }

    //LeaderboardGUI
    public void toggleLeaderboardGUI(bool reverseOrder,float delay, float timeGap)
    {
        LeaderboardGUI.GetComponent<LeaderboardManager>().beginToggleRanksOneByOneAfter(reverseOrder,delay,timeGap);
        LeaderboardGUIisHidden = LeaderboardGUIisHidden ? false : true;
    }

    public void toggleShopGUI(float delay)
    {
        ShopGUI.GetComponent<ShopManager>().toggleShopGUI(delay);
    }
    



}
