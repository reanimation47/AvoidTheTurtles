using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator pAnimator;

    void Start()
    {
        pAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        //Debug.Log(PlayerControl.Instance.moveSpeed);
        //float currentSpeed = PlayerControl.Instance.rb.velocity.magnitude;
        float currentSpeed = PlayerControl.Instance.moveSpeed;
        //Debug.Log(currentSpeed);

        if(currentSpeed <= 6)
        {
            pAnimator.SetFloat("Speed",0);
        }

        if(currentSpeed > 6 && currentSpeed <= 10 )
        {
            pAnimator.SetFloat("Speed", 0.5f);
        }

        if(currentSpeed > 10)
        {
            pAnimator.SetFloat("Speed",1f);
        }


    }
    // Vector3 oldPosition;
    // void FixedUpdate()
    // {
    //     float speed = Vector3.Distance(oldPosition, PlayerControl.Instance.transform.position) * 100f;
    //     oldPosition = PlayerControl.Instance.transform.position;

    //     Debug.Log("Speed: "+ speed.ToString("F2"));
    // }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Target")
        {
            pAnimator.SetTrigger("getTargetTrigger");
        }
    }


}
