using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTarget : MonoBehaviour
{
    public int target = 30;
     
     void Awake()
     {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;

        DontDestroyOnLoad(this.gameObject);
     }
     
     void Update()
     {
        if(Application.targetFrameRate != target)
        Application.targetFrameRate = target;
     }
}