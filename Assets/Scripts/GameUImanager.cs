using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUImanager : MonoBehaviour
{
    public static GameUImanager instance {get; private set;}

    [SerializeField] private GameObject energySlider;
    [SerializeField] private GameObject energySliderFill;
    Color energySliderFillColor;

    float energySliderValue;
    float energyPoints = 0;

    //Score
    [SerializeField] private GameObject ScoreDisplay;
    public TextMeshProUGUI scoreText;
    public bool ScorePop = false;
    Vector3 ScoreScaler = new Vector3(1,1,1);

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
        energySliderFillColor = energySliderFill.GetComponent<Image>().color;
        //energySliderValue = energySlider.GetComponent<Slider>().value;
        //energySliderFill.GetComponent<Image>().color = new Color(0,0,0);
    }

    void Update()
    {
        energySliderFill.GetComponent<Image>().color = energySliderFillColor;
        energyPoints = GameManager.instance.specialPoints;
        energySlider.GetComponent<Slider>().value= Mathf.Lerp(energySlider.GetComponent<Slider>().value, energyPoints, 0.05f);
        
        energySliderFillColor.r = Mathf.Lerp(energySliderFillColor.r, energyPoints/5, 0.05f);

        scoreText.text = GameManager.instance.points.ToString();


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
    }

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
}
