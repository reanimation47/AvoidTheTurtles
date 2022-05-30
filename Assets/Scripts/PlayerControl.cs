using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public static PlayerControl Instance;


    public float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject EnemyKilledParticle;


    public Rigidbody rb;

    float xInput;
    float zInput;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();

    }


    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");


    }

    void FixedUpdate()
    {
        float xVelocity = xInput * moveSpeed;
        float zVelocity = zInput * moveSpeed;

        Vector3 moveDirection = new Vector3(xVelocity, 0, zVelocity);

        rb.velocity = moveDirection;


        if(moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            GameObject.Find("GameManager").GetComponent<GameManager>().isOver = true;
        }

        if(collision.gameObject.tag == "VulEnemy")
        {
            //collision.gameObject.SetActive(false);

            //Knock vulnerable enemy away

            //Get collision direction

            Vector3 dir = collision.contacts[0].point -transform.position;
            EnemyKilledParticle.transform.position = collision.contacts[0].point;
            EnemyKilledParticle.GetComponent<ParticleSystem>().Play();

            dir = dir.normalized;
            Debug.Log(dir);
            collision.gameObject.GetComponent<EnemyNavMesh>().disableEverything();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir*150f, ForceMode.Impulse);
            //rb.AddForce(-dir*20f, ForceMode.Impulse);
        }
    }
}
