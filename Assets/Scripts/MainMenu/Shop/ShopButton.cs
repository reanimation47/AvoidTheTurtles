using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    void OnMouseDown()
    {
        MenuManager.instance.toggleShopGUI(1);
    }
}
