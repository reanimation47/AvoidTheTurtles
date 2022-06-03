using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUImanager : MonoBehaviour
{
    public static GameUImanager instance {get; private set;}

    //Audio
    public AudioClip FinalGemsPopSound;
    public AudioClip GameOverSound;
    //Energy slider
    [SerializeField] private GameObject energySlider;
    [SerializeField] private GameObject energySliderFill;
    Color energySliderFillColor;
    Vector3 energyBarScaler = new Vector3(1,1.5f,1);
    float energySliderValue;
    float energyPoints = 0;
    public bool energyBarWiggling = false;
    public bool energyBarWiggleOn = false;

    //Score
    [SerializeField] private GameObject ScoreDisplay;
    public TextMeshProUGUI scoreText;
    public bool ScorePop = false;
    Vector3 ScoreScaler = new Vector3(1,1,1);

    //HighScore
    [SerializeField] private TextMeshProUGUI currentHighScoreText;


    //Player's speed display
    public TextMeshProUGUI playerSpeedValue;

    //Game over GUI
    float GameOverGUIalpha = 0f;
    [SerializeField] private GameObject GameOverGUI;
    [SerializeField] private TextMeshProUGUI finalSpeed;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TextMeshProUGUI finalGems;
    [SerializeField] private GameObject finalGemsSection;
    Vector3 finalGemsSectionScaler = new Vector3(1,1,1);
    public bool finalGemsPop = false;
    float finalSpeedTarget = 0;
    float finalSpeedValue = 0;
    float finalScoreTarget = 0;
    float finalScoreValue = 0;
    
    public bool hideGameOverGUI = true;


    private void Awake()
    {
    // If there is an instance, and it's not me, delete myself.
        //
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
        energySliderFillColor = energySliderFill.GetComponent<Image>().color;
        //energySliderValue = energySlider.GetComponent<Slider>().value;
        //energySliderFill.GetComponent<Image>().color = new Color(0,0,0);
    }

    void Update()
    {
        playerSpeedValue.text = GameManager.instance.playerSpeedInInt.ToString();
        currentHighScoreText.text = MenuManager.instance.currentHighscore().ToString();
        //Animating Energy bar
        energySliderFill.GetComponent<Image>().color = energySliderFillColor;
        energyPoints = GameManager.instance.specialPoints;
        energySlider.GetComponent<Slider>().value= Mathf.Lerp(energySlider.GetComponent<Slider>().value, energyPoints, 0.05f);
        
        energySliderFillColor.r = Mathf.Lerp(energySliderFillColor.r, energyPoints/5, 0.05f);

        scoreText.text = GameManager.instance.points.ToString();

        //Animating Enery bar when energy reaches full
        energySlider.transform.localScale = energyBarScaler;
        if(energyPoints == 5 && !energyBarWiggling)
        {
            energyBarWiggling = true;
            StartCoroutine(wiggleEnergyBar(0.2f));
            
        }
        if(energyPoints != 5 && energyBarWiggling)
        {
            StopCoroutine(wiggleEnergyBar(0.2f));
            energyBarWiggleOn = false;
            energyBarWiggling = false;
        }
        if(energyBarWiggleOn)
        {
            energyBarScaler.x = Mathf.Lerp(energyBarScaler.x,1.1f,0.05f);
            energyBarScaler.y = Mathf.Lerp(energyBarScaler.y,1.6f,0.05f);
        }else
        {
            energyBarScaler.x = Mathf.Lerp(energyBarScaler.x,1f,0.05f);
            energyBarScaler.y = Mathf.Lerp(energyBarScaler.y,1.5f,0.05f);
        }


        //Animating current score
        ScoreDisplay.transform.localScale = ScoreScaler;
        if(ScorePop)
        {
            ScoreScaler.x = Mathf.Lerp(ScoreScaler.x, 1.3f, 0.05f);
            ScoreScaler.y = Mathf.Lerp(ScoreScaler.y, 1.3f, 0.05f);
        }else
        {
            ScoreScaler.x = Mathf.Lerp(ScoreScaler.x, 1f, 0.05f);
            ScoreScaler.y = Mathf.Lerp(ScoreScaler.y, 1f, 0.05f);
        } 

        //Animating Game Over UI
        GameOverGUI.GetComponent<CanvasGroup>().alpha = GameOverGUIalpha;
        finalSpeed.text = (((int)finalSpeedValue)+1).ToString();
        finalScore.text = (((int)finalScoreValue)+1).ToString();

        finalGemsSection.transform.localScale = finalGemsSectionScaler;
    }

    
    void FixedUpdate()
    {
        //Debug.Log(GameOverGUIalpha);
        //Showing and hiding Game Over UI
        if(hideGameOverGUI)
        {
            GameOverGUIalpha = Mathf.Lerp(GameOverGUIalpha, 0f, 0.025f);
        }else
        {
            GameOverGUIalpha = Mathf.Lerp(GameOverGUIalpha, 1f, 0.025f);
        }

        finalScoreValue = Mathf.Lerp(finalScoreValue, finalScoreTarget, 0.025f);
        finalSpeedValue = Mathf.Lerp(finalSpeedValue, finalSpeedTarget, 0.025f);

        //Poping final gems reward

        if(finalGemsPop)
        {
            finalGemsSectionScaler.x = Mathf.Lerp(finalGemsSectionScaler.x, 1.4f, 0.1f);
            finalGemsSectionScaler.y = Mathf.Lerp(finalGemsSectionScaler.y, 1.4f, 0.1f);
        }else
        {
            finalGemsSectionScaler.x = Mathf.Lerp(finalGemsSectionScaler.x, 1f, 0.1f);
            finalGemsSectionScaler.y = Mathf.Lerp(finalGemsSectionScaler.y, 1f, 0.1f);
        }


    }

    //In game GUI
    IEnumerator wiggleScoreDisplay()
    {
        ScorePop = true;
        yield return new WaitForSeconds(0.2f);
        ScorePop = false;
    }

    public void startWigglingScoreDisplay()
    {
        StartCoroutine(wiggleScoreDisplay());
    }

    IEnumerator wiggleEnergyBar(float timeGap)
    {
        while(energyBarWiggling)
        {
            energyBarWiggleOn = energyBarWiggleOn ? false : true;
            yield return new WaitForSeconds(timeGap);
        }
        
    }


    //Game Over GUI
    public void toggleGameOverGUI()
    {
        hideGameOverGUI = hideGameOverGUI ? false : true;
        toggleFinalResultsValues();
        //AudioManager.instance.playSound(GameOverSound,1f);
        playGameOverSoundAfter(0.5f);
        
    }

    public void toggleFinalResultsValues()
    {
        if(!hideGameOverGUI)
        {
            finalScoreTarget = GameManager.instance.points;
            finalSpeedTarget = GameManager.instance.playerSpeedInInt;
            startWigglingFinalGems(4f);
        }else
        {
            finalScoreTarget = 0;
            finalSpeedTarget = 0;
        }
    }
    IEnumerator wiggleFinalGems()
    {
        finalGemsPop = true;
        yield return new WaitForSeconds(0.2f);
        finalGemsPop = false;
    }

    IEnumerator wiggleFinalGemsAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(wiggleFinalGems());
        DisplayFinalGems();
    }

    public void startWigglingFinalGems(float delay)
    {
        StartCoroutine(wiggleFinalGemsAfter(delay));
    }

    void DisplayFinalGems()
    {
        finalGems.text = (finalScoreTarget*finalSpeedTarget).ToString();
        AudioManager.instance.playSound(FinalGemsPopSound,1f);
    }

    IEnumerator playGameOverSoundAfterIE(float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioManager.instance.playSound(GameOverSound,1f);
    }

    void playGameOverSoundAfter(float delay)
    {
        StartCoroutine(playGameOverSoundAfterIE(delay));
    }


}
