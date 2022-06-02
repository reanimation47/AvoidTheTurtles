using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject ComingSoonText;
    [SerializeField] private GameObject BackButton;
    public bool hideComingSoonText = false;
    public bool hideBackButton = false;
    Vector3 ComingSoonTextPositioner;
    Vector3 BackButtonPositioner;

    Vector3 defaultComingSoonTextPosition;
    Vector3 defaultBackButtonPosition;

    void Start()
    {
        ComingSoonTextPositioner = ComingSoonText.transform.position;
        BackButtonPositioner = BackButton.transform.position;

        defaultComingSoonTextPosition = ComingSoonText.transform.position;
        defaultBackButtonPosition = BackButton.transform.position;
    }

    void Update()
    {
        ComingSoonText.transform.position = ComingSoonTextPositioner;
        BackButton.transform.position = BackButtonPositioner;

        if(hideComingSoonText)
        {
            ComingSoonTextPositioner.x = Mathf.Lerp(ComingSoonTextPositioner.x,defaultComingSoonTextPosition.x +15f,0.05f);
            BackButtonPositioner.x = Mathf.Lerp(BackButtonPositioner.x, defaultBackButtonPosition.x + 15f, 0.05f);
        }else
        {
            ComingSoonTextPositioner.x = Mathf.Lerp(ComingSoonTextPositioner.x,defaultComingSoonTextPosition.x,0.05f);
            BackButtonPositioner.x = Mathf.Lerp(BackButtonPositioner.x, defaultBackButtonPosition.x, 0.05f);
        }
    }

    public void toggleShopGUI(float delay)
    {
        StartCoroutine(toggleShopGUIIE(delay));
    }

    IEnumerator toggleShopGUIIE(float delay)
    {
        yield return new WaitForSeconds(delay);
        hideComingSoonText = hideComingSoonText ? false : true;
    }
    
}
