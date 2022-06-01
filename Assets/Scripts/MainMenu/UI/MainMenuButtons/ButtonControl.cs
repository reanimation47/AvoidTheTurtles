using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public bool mouseHover = false;
    public GameObject Text;
    Vector3 scaleUp = new Vector3(3f,3f,3f);
    Vector3 scaler;
    Vector3 defaultScale;

    public bool isWiggling = false;
    public bool wiggleOn = false;

    void Start()
    {
        scaler = transform.localScale;
        defaultScale = transform.localScale;
    }
    void OnMouseOver()
    {
        mouseHover = true;
        Text.GetComponent<ShowText>().showText = true;
    }

    void OnMouseExit()
    {
        mouseHover = false;
        Text.GetComponent<ShowText>().showText = false;
    }

    void OnMouseDown()
    {
        MenuManager.instance.onClickEffect(transform.position);
        MenuManager.instance.toggleFrontGUIafter(0.5f);
        StartCoroutine(startWiggling(2f));
    }

    void Update()
    {
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
