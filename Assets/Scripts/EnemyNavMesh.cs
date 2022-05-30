using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    private Animator eAnimator;

    NavMeshAgent navAgent;
    GameObject player;
    Vector3 destination;

    bool onStealth;
    bool isCharging = false;
    ParticleSystem particle;

    public Rigidbody rb;
    public float chargeSpeed;
    public float chargeDelay;
    public float chargeTime;
    public float vulTimer;
    public float waitT;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("GameManager").GetComponent<GameManager>().player;
        waitT = chargeTime + chargeDelay;
    }

    void Start()
    {
        eAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        particle = GetComponent<ParticleSystem>();
        StartCoroutine("newDestination");
    }

    void OnEnable()
    {
        gameObject.tag = "Enemy";
        isCharging = false;
        navAgent.isStopped = false;
        rb.isKinematic = true;
        eAnimator.SetBool("isVulnerable", false);
        eAnimator.SetBool("startCharging", false);
        GetComponent<BoxCollider>().isTrigger = false;
    }

    void Update()
    {


        onStealth = GameObject.Find("GameManager").GetComponent<GameManager>().onStealth;
        //Debug.Log(onStealth);
        if(player && !onStealth)
        {
            navAgent.destination = player.transform.position;
            eAnimator.SetBool("playerOnStealth", false);
        }
        if(player && onStealth)
        {
            navAgent.destination = destination;
            eAnimator.SetBool("playerOnStealth", true);
        }

    }

    void LateUpdate()
    {
        if(!player){return;}

        if(onStealth){return;}
        if(isCharging)
        {
            transform.LookAt(player.transform);
        }

        //Debug.Log(transform.forward);

    }

    public void useChargeSkill()
    {
        
        isCharging = true;
        //rb.isKinematic = false
        StartCoroutine("startCharging");
        StartCoroutine(becomeVulnerable());
        StartCoroutine(endCharging());
    }

    //public void StartNavMesh()
    //{
        //StartCoroutine(startNavMesh());
    //}

    IEnumerator newDestination()
    {
        while(!GameObject.Find("GameManager").GetComponent<GameManager>().isOver)
        {
            destination = GameObject.Find("GameManager").GetComponent<GameManager>().TargetDestination();
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator startCharging()
    {
        navAgent.isStopped = true;
        //Start charging
        eAnimator.SetBool("startCharging", true);
        particle.Play();
        yield return new WaitForSeconds(chargeTime);
        particle.Stop();
        eAnimator.SetBool("startCharging", false);
        
        rb.AddForce(transform.forward *chargeSpeed, ForceMode.Impulse );

    }

    IEnumerator becomeVulnerable()
    {
        yield return  new WaitForSeconds(chargeTime + chargeDelay);
        eAnimator.SetBool("isVulnerable", true);
        gameObject.tag = "VulEnemy";
        yield return new WaitForSeconds(vulTimer);
        eAnimator.SetBool("isVulnerable", false);
    }

    IEnumerator endCharging()
    {

        yield return new WaitForSeconds(chargeTime + chargeDelay + vulTimer);
        gameObject.tag = "Enemy";
        isCharging = false;
        navAgent.isStopped = false;


    }

    IEnumerator inactiveAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    public void disableEverything()
    {
        navAgent.isStopped = true;
        rb.isKinematic = false;
        GetComponent<BoxCollider>().isTrigger = true;
        StartCoroutine(inactiveAfterSeconds(3f));
    }
}
