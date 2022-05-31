using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Prefabs and refs")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject Target;
    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject Danger;
    [SerializeField] private Text Score;
    [SerializeField] private Text HighScore;
    [SerializeField] private Text playerSpeed;
    [SerializeField] private Text SpecialPoints;
    [SerializeField] private Text PressSpace;
    [SerializeField] private GameObject RestartButton;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject MenuButton;

    [Header("LevelOne Settings")]
    [SerializeField] private float stealthTimer;
    [SerializeField] private float levelTwoThreshold;
    [SerializeField] private float timeBetweenCharges;

    [Header("Others")]
    //[HideInInspector]
    public float minX, maxX, minY, maxY;
    float kinematicTimer = 0;
    bool onKinematicTimer = false;
    public int points = 0;
    public bool isOver = false;
    bool showRestartButton = false;

    int specialPoints = 0;

    public GameObject player;
    GameObject target;
    GameObject xWall1,xWall2,zWall1,zWall2;
    //GameObject enemy;
    Vector3 screenPos;

    Vector3 playerTransform;

    Vector3 maxScreenBounds;
    Vector3 minScreenBounds;

    Color playerColor;

    public bool onStealth = false;
    public bool aboveTen = false;
    bool levelTwo = false;


    void Awake()
    {
        //Restart Button
        RestartButton.GetComponent<Button>().onClick.AddListener(RestartGame);
        ExitButton.GetComponent<Button>().onClick.AddListener( ()=>
        {
            Application.Quit();
        } );
        MenuButton.GetComponent<Button>().onClick.AddListener( ()=>
        {
            SceneManager.LoadScene("MainMenuScene");
        }  );
    }

    void Start()
    {
        //player = Instantiate(Player, new Vector3( RandomPos(10) , 0.5f, RandomPos(10) ),  Quaternion.identity);
        //SpecialPoints.color = new Color(1f,0.5f,0.7f);
        PressSpace.color = new Color(244/255f,96/255f,54/255f,0f);

        playerColor = playerBody.GetComponent<SkinnedMeshRenderer>().material.color;


        target = Instantiate(Target, new Vector3( 0 , 1.5f, 0 ),  Quaternion.identity);
        target.SetActive(false);

        playerTransform = player.transform.position;

        float camDistance = Vector3.Distance(player.transform.position, Camera.main.transform.position);
        Vector3 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0,0, camDistance));
        Vector3 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1,1, camDistance));

        minX = bottomCorner.x+1;
        maxX = topCorner.x-1;
        minY = bottomCorner.z+1;
        maxY = topCorner.z-1;

        xWall1 = Instantiate(Wall, new Vector3(minX, -2.5f, 0), Quaternion.identity);
        xWall2 = Instantiate(Wall, new Vector3(maxX, -2.5f, 0), Quaternion.identity);
        zWall1 = Instantiate(Wall, new Vector3(0, -2.5f, minY), transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        zWall2 = Instantiate(Wall, new Vector3(0, -2.5f, maxY), transform.rotation * Quaternion.Euler(0f, 90f, 0f));

        
        //Instantiate(Enemy, new Vector3( RandomPos(maxX) , 0.5f, RandomPos(maxY) ),  Quaternion.identity);
        SpawnTarget();
        //StartCoroutine("SpawnEnemies");
        StartCoroutine("decreasePlayerSpeed");
        StartCoroutine(decreaseKinematicTimer());

    }
    bool isBlinking = false;
    GameObject[] enemies;
    void Update()
    {
        //Debug.Log(kinematicTimer);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            useStealth();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            //StartCoroutine(toggleObjectsKinematic(enemies,false,0f));
            int randomEnemy = Random.Range(0, enemies.Length);

            //enemies[randomEnemy].SetActive(false);
            EnemyNavMesh choosenEnemy = enemies[randomEnemy].GetComponent<EnemyNavMesh>();
            choosenEnemy.useChargeSkill();
            float waitT = choosenEnemy.waitT;
            kinematicTimer = waitT;
            //StartCoroutine(toggleObjectsKinematic(enemies,true,waitT));
            //choosenEnemy.StartNavMesh();
        }
        //Debug.Log(kinematicTimer);

        if(points >= levelTwoThreshold && !levelTwo )
        {
            StartCoroutine(beginLevelTwo());
            levelTwo = true;
        }


        if(specialPoints == 5 && !isBlinking)
        {
            StartCoroutine("fullSpecialPoints");
            isBlinking = true;
        }
        if(specialPoints != 5 && isBlinking)
        {
            StopCoroutine("fullSpecialPoints");
            PressSpace.color = new Color(244/255f,96/255f,54/255f,0f);
            SpecialPoints.color = new Color(157/225f,187/255f,243/255f);
            isBlinking = false;
        }

        //Debug.Log(onStealth);
        //update current score and speed
        DisplayScore();
        DisplayerSpeed();
        DisplaySpecialPoints();
        DisplayHighScore();
        //if(player)
      //  {
            // Get current position
            //Vector3 pos = player.transform.position;

            // Horizontal constraint
            //if(pos.x < minX) pos.x = minX;
            //if(pos.x > maxX) pos.x = maxX;

             // vertical constraint
          //  if(pos.z < minY) pos.z = minY;
          //  if(pos.z > maxY) pos.z = maxY;

            // Update position
            //player.transform.position = pos;
        //}

        MenuManager.instance.checkHighScore(points);

    }

    void FixedUpdate()
    {
        if(isOver && !showRestartButton)
        {
            RestartButton.SetActive(true);
            ExitButton.SetActive(true);
            MenuButton.SetActive(true);
            showRestartButton = true;
        }

        if(!player){return;}

        if(kinematicTimer>0 && !onKinematicTimer)
        {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            StartCoroutine(toggleObjectsKinematic(enemies,false,0f));
            onKinematicTimer = true;
        }

        if(kinematicTimer<=0 && onKinematicTimer)
        {
          enemies = GameObject.FindGameObjectsWithTag("Enemy");
          StartCoroutine(toggleObjectsKinematic(enemies,true,0f));
          onKinematicTimer = false;
        }

        // if(player.GetComponent<PlayerControl>().moveSpeed >= 10f)
        // {
        //     aboveTen = true;
        // }

        // if(player.GetComponent<PlayerControl>().moveSpeed < 10)
        // {
        //     aboveTen = false;
        // }

    }

    IEnumerator SpawnEnemies()
    {
        // while(!isOver)
        // {

        // }
        float newPosX = RandomPos(maxX) -2f;
        float newPosY = RandomPos(maxY -2f);

        yield return new WaitForSeconds(.1f);

        //GameObject danger = Instantiate(Danger, new Vector3( newPosX , 1.5f, newPosY ),  Quaternion.identity);
        GameObject danger = DangerPool.SharedInstance.GetPooledObject();
        if (danger != null) {
            danger.transform.position = new Vector3( newPosX , -2.5f, newPosY );
            danger.transform.rotation = Quaternion.identity;
            danger.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

            //Destroy(danger);
        danger.SetActive(false);

            //Instantiate(Enemy, new Vector3( newPosX , 1.5f, newPosY ),  Quaternion.identity);

            //Using Enemies Pool
        GameObject enemy = EnemyPool.SharedInstance.GetPooledObject();

        if (enemy != null) {
            enemy.transform.position = new Vector3( newPosX , 1.5f, newPosY );
            enemy.transform.rotation = Quaternion.identity;
            enemy.SetActive(true);
        }else
        {
            Instantiate(Enemy, new Vector3( newPosX , 1.5f, newPosY ),  Quaternion.identity);
        }
    }

    public void SpawnOneEnemy()
    {
        StartCoroutine("SpawnEnemies");
    }

    public void SpawnTarget()
    {
        float newPosX = RandomPos(maxX) -2;
        float newPosY = RandomPos(maxY) -2;

        target.transform.position = new Vector3(newPosX, 1.5f, newPosY);
        target.SetActive(true);
        target.GetComponent<TargetNav>().startNewDestination();

    }

    public void hideTarget()
    {
        target.SetActive(false);
    }


    IEnumerator decreasePlayerSpeed()
    {
        while(!isOver && player)
        {
            player.GetComponent<PlayerControl>().moveSpeed -= 0.1f;
            yield return new WaitForSeconds(1f);


        }
    }

    public void increaseDifficulty()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject e in enemies)
        {
            e.GetComponent<NavMeshAgent>().speed += 0.5f;
        }

        player.GetComponent<PlayerControl>().moveSpeed +=1;
        target.GetComponent<NavMeshAgent>().speed +=0.5f;
    }

    void DisplayScore()
    {
        Score.text = "Score: " + points;
    }

    void DisplayHighScore()
    {
        int currentHighScore = MenuManager.instance.currentHighscore();
        HighScore.text = "HighScore: "+ currentHighScore;
    }

    void DisplayerSpeed()
    {
        if(player)
        {
            float f = player.GetComponent<PlayerControl>().moveSpeed;

            //Rounding up number
            f = Mathf.Round(f * 10.0f) * 0.1f;
            playerSpeed.text = "Current player's speed: " + f;
        }

    }

    void DisplaySpecialPoints()
    {
        SpecialPoints.text = "Ability: "+ specialPoints + "/5";
    }


    float RandomPos(float range)
    {
        float rand = Random.Range(-range, range);
        return rand;
    }

    void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public Vector3 TargetDestination()
    {
        float newPosX = RandomPos(maxX)-2;
        float newPosY = RandomPos(maxY)-2;
        //Debug.Log(newPosX + " " + newPosY);

        return new Vector3(newPosX, -2.5f, newPosY);
    }

    public void targetEaten()
    {
        points++;
        if(specialPoints < 5)
        {
            specialPoints++;
        }
    }

    void useStealth()
    {
        if(specialPoints == 5)
        {
            specialPoints = 0;
            StartCoroutine("beginStealth");

        }

    }

    IEnumerator beginStealth()
    {
        if(player)
        {
          onStealth = true;
          playerColor.a = 0.1f;
          playerBody.GetComponent<SkinnedMeshRenderer>().material.color = playerColor;

          yield return new WaitForSeconds(stealthTimer);

          playerColor.a = 1f;
          playerBody.GetComponent<SkinnedMeshRenderer>().material.color = playerColor;
          onStealth = false;
        }

    }

    IEnumerator fullSpecialPoints()
    {
        while(!isOver)
        {
            //Color oldColor = SpecialPoints.color;
            SpecialPoints.color = new Color(244/255f,96/255f,54/255f);
            PressSpace.color = new Color(244/255f,96/255f,54/255f,1f);

            yield return new WaitForSeconds(0.5f);
            SpecialPoints.color = new Color(157/225f,187/255f,243/255f);
            PressSpace.color = new Color(244/255f,96/255f,54/255f,0f);

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator toggleObjectsKinematic(GameObject[] objects, bool toggle, float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
        foreach(GameObject o in objects)
        {

            o.GetComponent<Rigidbody>().isKinematic = toggle;
        }
    }

    IEnumerator decreaseKinematicTimer()
    {
        while(!isOver)
        {
              yield return new WaitForSeconds(1f);
              if(kinematicTimer>0)
              {
                  kinematicTimer--;
              }
        }


    }

    IEnumerator beginLevelTwo()
    {
        while(!isOver && player)
        {
            yield return new WaitForSeconds(timeBetweenCharges);
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int randomEnemy = Random.Range(0, enemies.Length);

            EnemyNavMesh choosenEnemy = enemies[randomEnemy].GetComponent<EnemyNavMesh>();
            choosenEnemy.useChargeSkill();
            float waitT = choosenEnemy.waitT;
            kinematicTimer = waitT;

            //Decrease time between charges after every charge

            if(timeBetweenCharges <=0){yield break;}
            if(timeBetweenCharges >1f)
            {
                timeBetweenCharges -= 0.2f;
            }

            if(timeBetweenCharges < 1f )
            {
                timeBetweenCharges -= 0.1f;
            }

        }
    }



}
