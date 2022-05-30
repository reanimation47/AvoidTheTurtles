using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetNav : MonoBehaviour
{
    NavMeshAgent navAgent;
    Vector3 destination;
    GameObject particle;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        particle = GameObject.Find("TargetParticle");
        //StartCoroutine("newDestination");
    }

    void Update()
    {
        navAgent.destination = destination;
    }

    IEnumerator newDestination()
    {
        while(!GameObject.Find("GameManager").GetComponent<GameManager>().isOver)
        {
            destination = GameObject.Find("GameManager").GetComponent<GameManager>().TargetDestination();
            yield return new WaitForSeconds(1f);
        }
    }

    public void startNewDestination()
    {
        StartCoroutine("newDestination");
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            particle.transform.position = transform.position;
            particle.GetComponent<ParticleSystem>().Play();
            //Destroy(gameObject);
            //gameObject.SetActive(false);
            GameObject.Find("GameManager").GetComponent<GameManager>().hideTarget();
            //GameObject.Find("GameManager").GetComponent<GameManager>().removeTarget();

            GameObject.Find("GameManager").GetComponent<GameManager>().targetEaten();
            GameObject.Find("GameManager").GetComponent<GameManager>().SpawnTarget();
            GameObject.Find("GameManager").GetComponent<GameManager>().increaseDifficulty();
            GameObject.Find("GameManager").GetComponent<GameManager>().SpawnOneEnemy();
        }
    }
}
