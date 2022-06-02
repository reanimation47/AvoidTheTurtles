using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardGoBackButton : MonoBehaviour
{

    public bool mouseHover = false;
    Vector3 scaler;
    Vector3 defaultScale;

    public bool isWiggling = false;
    public bool wiggleOn = false;


    void Start()
    {
        scaler = transform.localScale;
        defaultScale = transform.localScale;
    }
    void OnMouseDown()
    {
        StartCoroutine(startWiggling(0.2f));
        MenuManager.instance.onClickEffect(transform.position);
        MenuManager.instance.toggleLeaderboardGUI(true,0.5f,0.2f);
        MenuManager.instance.toggleFrontGUIafter(1f);
    }

    void OnMouseOver()
    {
        mouseHover = true;
    }

    void OnMouseExit()
    {
        mouseHover = false;
    }

    void Update()
    {   
        //Hover animation
        transform.localScale = scaler;
        if(mouseHover)
        {
            scaler.x = Mathf.Lerp(scaler.x , defaultScale.x + 0.2f, 0.03f );
            scaler.y = Mathf.Lerp(scaler.y , defaultScale.y + 0.2f, 0.03f );
        }else
        {
            scaler.x = Mathf.Lerp(scaler.x , defaultScale.x , 0.03f );
            scaler.y = Mathf.Lerp(scaler.y , defaultScale.y , 0.03f );
        }

        //On click animation
        if(isWiggling)
        {
            if(wiggleOn)
            {
                scaler.x = Mathf.Lerp(scaler.x , defaultScale.x + 1f, 0.03f );
            }else
            {
                scaler.x = Mathf.Lerp(scaler.x , defaultScale.x, 0.03f );
            }
        }
    }
    IEnumerator startWiggling(float duration)
    {
        isWiggling = true;
        StartCoroutine(wigglingAction(0.1f));
        yield return new WaitForSeconds(duration);
        isWiggling = false;
        StopCoroutine(wigglingAction(0.1f));

    }

    IEnumerator wigglingAction(float rate)
    {
        while(isWiggling)
        {
            yield return new WaitForSeconds(rate);
            wiggleOn = wiggleOn ? false : true;
        }

    }
}
