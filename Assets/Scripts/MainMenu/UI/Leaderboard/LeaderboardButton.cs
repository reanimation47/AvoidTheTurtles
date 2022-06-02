using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardButton : MonoBehaviour
{
    // Start is called before the first frame update
    void OnMouseDown()
    {
        MenuManager.instance.toggleLeaderboardGUI(false,1f,0.5f);
    }
}
