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
        MenuManager.instance.toggleFrontGUIafter(0.3f);
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
    }
}
