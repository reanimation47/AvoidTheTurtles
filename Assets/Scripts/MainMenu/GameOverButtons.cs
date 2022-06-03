using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverButtons : MonoBehaviour
{
    public AudioClip clickSound;
    public bool mouseHover = false;
    //public GameObject Text;
    Vector3 scaler;
    Vector3 defaultScale;

    public bool isWiggling = false;
    public bool wiggleOn = false;

    public bool isReplayButton = false;

    void Start()
    {
        scaler = transform.localScale;
        defaultScale = transform.localScale;
    }
    void OnMouseOver()
    {
        mouseHover = true;
        //Text.GetComponent<ShowText>().showText = true;
    }

    void OnMouseExit()
    {
        mouseHover = false;
        //Text.GetComponent<ShowText>().showText = false;
    }

    void OnMouseDown()
    {
        //MenuManager.instance.onClickEffect(transform.position);
        //MenuManager.instance.toggleFrontGUIafter(0.5f);
        StartCoroutine(startWiggling(0.3f));
        AudioManager.instance.playSound(clickSound,1f);
        if(isReplayButton)
        {
            restartGame(0.5f);
        }else
        {
            backToMenu(0.5f);
        }
    }

    void Update()
    {
        transform.localScale = scaler;
        
    }

    void FixedUpdate()
    {
        if(mouseHover)
        {
            scaler.x = Mathf.Lerp(scaler.x , defaultScale.x + 0.1f, 0.15f );
            scaler.y = Mathf.Lerp(scaler.y , defaultScale.y + 0.1f, 0.15f );
        }else
        {
            scaler.x = Mathf.Lerp(scaler.x , defaultScale.x , 0.15f );
            scaler.y = Mathf.Lerp(scaler.y , defaultScale.y , 0.15f );
        }

        if(isWiggling)
        {
            if(wiggleOn)
            {
                scaler.x = Mathf.Lerp(scaler.x , defaultScale.x + 0.1f, 0.15f );
            }else
            {
                scaler.x = Mathf.Lerp(scaler.x , defaultScale.x, 0.15f );
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

    IEnumerator restartGameIE(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("SampleScene");
    }

    void restartGame(float delay)
    {
        StartCoroutine(restartGameIE(delay));
    }

    IEnumerator backToMenuIE(float delay)
    {
        yield return new WaitForSeconds(delay);
        MenuManager.instance.DestroyMyself();
        SceneManager.LoadScene("MainMenuScene");
    }

    void backToMenu(float delay)
    {
        StartCoroutine(backToMenuIE(delay));
    }

}
