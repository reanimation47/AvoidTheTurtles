using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotation : MonoBehaviour
{
    public float spinSpeed;
    bool LeaderboardGUIisHidden = true;


    void Update()
    {
        Vector3 mouse_pos = Input.mousePosition;
        transform.LookAt(mouse_pos + new Vector3(0,0,-2000f));
        //transform.Rotate(0, spinSpeed * Time.deltaTime, 0);


    }
}
