using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShowText : MonoBehaviour
{
    public bool showText = false;
    TextMeshProUGUI text;
    Color textShown;
    Vector3 textPosition;
    public Transform ButtonPos;
    public float textMoveDistance = 1.5f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        // Color color = text.color;
        // color.a = 0f;
        // text.color = color;

        textShown = text.color;
        textPosition = ButtonPos.position;



    }

    void Update()
    {

        text.color = textShown;
        transform.position = textPosition;
        

    }

    void FixedUpdate()
    {
        if(showText)
        {
            textShown.a = Mathf.Lerp(textShown.a, 1f, 0.15f);
            textPosition.y = Mathf.Lerp(textPosition.y, ButtonPos.position.y +textMoveDistance, 0.15f);
        }else
        {
            textShown.a = Mathf.Lerp(textShown.a, 0f, 0.15f);
            textPosition.y = Mathf.Lerp(textPosition.y, ButtonPos.position.y, 0.15f);
        }
    }
}
