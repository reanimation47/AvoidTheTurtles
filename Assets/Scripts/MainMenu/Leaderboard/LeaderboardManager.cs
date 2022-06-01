using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject Rank1;
    [SerializeField] private GameObject Rank2;
    [SerializeField] private GameObject Rank3;
    [SerializeField] private GameObject BackButton;
    [SerializeField] private TextMeshProUGUI Rank1Score;
    [SerializeField] private TextMeshProUGUI Rank2Score;
    [SerializeField] private TextMeshProUGUI Rank3Score;
    public bool hideRank1 = false;
    public bool hideRank2 = false;
    public bool hideRank3 = false;
    public bool hideBackButton = false;

    Vector3 Rank1Positioner;
    Vector3 Rank2Positioner;
    Vector3 Rank3Positioner;
    Vector3 BackButtonPositioner;

    Vector3 defaultRank1position;
    Vector3 defaultRank2position;
    Vector3 defaultRank3position;
    Vector3 defaultBackButtonPosition;

    void Start()
    {
        Rank1Positioner = Rank1.transform.position;
        Rank2Positioner = Rank2.transform.position;
        Rank3Positioner = Rank3.transform.position;
        BackButtonPositioner = BackButton.transform.position;

        defaultRank1position = Rank1.transform.position;
        defaultRank2position = Rank2.transform.position;
        defaultRank3position = Rank3.transform.position;
        defaultBackButtonPosition = BackButton.transform.position;

        Rank1Score.text = MenuManager.instance.currentHighscore().ToString();
    }

    void Update()
    {
        Rank1.transform.position = Rank1Positioner;
        Rank2.transform.position = Rank2Positioner;
        Rank3.transform.position = Rank3Positioner;
        BackButton.transform.position = BackButtonPositioner;

        if(hideRank1)
        {
            Rank1Positioner.x = Mathf.Lerp(Rank1Positioner.x, defaultRank1position.x -30f , 0.04f);
        }else
        {
            Rank1Positioner.x = Mathf.Lerp(Rank1Positioner.x, defaultRank1position.x , 0.04f);
        }
        if(hideRank2)
        {
            Rank2Positioner.x = Mathf.Lerp(Rank2Positioner.x, defaultRank2position.x -30f , 0.04f);
        }else
        {
            Rank2Positioner.x = Mathf.Lerp(Rank2Positioner.x, defaultRank2position.x , 0.04f);
        }
        if(hideRank3)
        {
            Rank3Positioner.x = Mathf.Lerp(Rank3Positioner.x, defaultRank3position.x -30f , 0.04f);
        }else
        {
            Rank3Positioner.x = Mathf.Lerp(Rank3Positioner.x, defaultRank3position.x , 0.04f);
        }
        if(hideBackButton)
        {
            BackButtonPositioner.x = Mathf.Lerp(BackButtonPositioner.x, defaultBackButtonPosition.x -30f, 0.04f);
        }else
        {
            BackButtonPositioner.x = Mathf.Lerp(BackButtonPositioner.x, defaultBackButtonPosition.x, 0.04f);
        }
    }

    IEnumerator toggleRanksOneByOneAfter(float delay,float timeGap)
    {
        yield return new WaitForSeconds(delay);
        hideRank1 = hideRank1 ? false : true;
        yield return new WaitForSeconds(timeGap);
        hideRank2 = hideRank2 ? false : true;
        yield return new WaitForSeconds(timeGap);
        hideRank3 = hideRank3 ? false : true;
        yield return new WaitForSeconds(timeGap);
        hideBackButton = hideBackButton ? false : true;
    }

    public void beginToggleRanksOneByOneAfter(float delay, float timeGap)
    {
        StartCoroutine(toggleRanksOneByOneAfter(delay,timeGap));
    }
}
