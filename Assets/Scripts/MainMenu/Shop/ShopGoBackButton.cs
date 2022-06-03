using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopGoBackButton : MonoBehaviour
{

    public bool mouseHover = false;
    Vector3 scaler;
    Vector3 defaultScale;

    public bool isWiggling = false;
    public bool wiggleOn = false;
    public AudioClip clickSound;


    void Start()
    {
        scaler = transform.localScale;
        defaultScale = transform.localScale;
    }
    void OnMouseDown()
    {
        StartCoroutine(startWiggling(0.2f));
        MenuManager.instance.onClickEffect(transform.position);
        MenuManager.instance.toggleShopGUI(0.3f);
        MenuManager.instance.toggleFrontGUIafter(1f);
        AudioManager.instance.playSound(clickSound,1f);
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
        
    }

    void FixedUpdate()
    {
        if(mouseHover)
        {
            scaler.x = Mathf.Lerp(scaler.x , defaultScale.x + 0.2f, 0.15f );
            scaler.y = Mathf.Lerp(scaler.y , defaultScale.y + 0.2f, 0.15f );
        }else
        {
            scaler.x = Mathf.Lerp(scaler.x , defaultScale.x , 0.15f );
            scaler.y = Mathf.Lerp(scaler.y , defaultScale.y , 0.15f );
        }

        //On click animation
        if(isWiggling)
        {
            if(wiggleOn)
            {
                scaler.x = Mathf.Lerp(scaler.x , defaultScale.x + 1f, 0.15f );
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
}
