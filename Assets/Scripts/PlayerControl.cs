using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public static PlayerControl Instance;

    public AudioClip playerDeadSound;
    public AudioClip playerKillsEnemySound;
    public float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject EnemyKilledParticle;
    [SerializeField] private GameObject PlayerKilledParticle;


    public Rigidbody rb;
    ParticleSystem playerParticle;

    bool particlePlaying = false;

    float xInput;
    float zInput;
    Vector3 moveDirection;

    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();


    }

    void Start()
    {
        playerParticle = GetComponent<ParticleSystem>();
    }


    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
        if(moveDirection != Vector3.zero && !particlePlaying)
        {
            playerParticle.Play();
            particlePlaying = true;
        }
        if(moveDirection == Vector3.zero && particlePlaying)
        {
            playerParticle.Stop();
            particlePlaying = false;
        }


    }

    void FixedUpdate()
    {
        float xVelocity = xInput * moveSpeed;
        float zVelocity = zInput * moveSpeed;

        moveDirection = new Vector3(xVelocity, 0, zVelocity);
        //Debug.Log(moveDirection);

        rb.velocity = moveDirection;

        //Debug.Log(moveDirection != Vector3.zero);
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
            AudioManager.instance.playSound(playerDeadSound,1f);
            PlayerKilledParticle.transform.position = collision.contacts[0].point + new Vector3(0,1,0);
            PlayerKilledParticle.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
            GameObject.Find("GameManager").GetComponent<GameManager>().isOver = true;
        }

        if(collision.gameObject.tag == "VulEnemy")
        {
            //collision.gameObject.SetActive(false);

            //Knock vulnerable enemy away

            //Get collision direction
            AudioManager.instance.playSound(playerKillsEnemySound,1f);
            Vector3 dir = collision.contacts[0].point -transform.position;
            EnemyKilledParticle.transform.position = collision.contacts[0].point + new Vector3(0,1,0);
            EnemyKilledParticle.GetComponent<ParticleSystem>().Play();

            dir = dir.normalized;
            //Debug.Log(dir);
            collision.gameObject.GetComponent<EnemyNavMesh>().disableEverything();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir*150f, ForceMode.Impulse);
            //rb.AddForce(-dir*20f, ForceMode.Impulse);
        }
    }
}
