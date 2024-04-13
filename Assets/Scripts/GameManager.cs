using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    public Transform[] MovePaternPoints0;
    public Transform[] MovePaternPoints1;
    public Transform[] itemWayPoints;

    public float maxSpawnDelay = 5f;
    private float curSpawnelay = 3f;

    private int maxEnemy = 10;
    public GameObject player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    private void Awake()
    {
/*        if(Instance == null) //싱글턴 오브젝트 생성
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
*/    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(this.GenerateEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        curSpawnelay += Time.deltaTime;

        if (curSpawnelay >= maxSpawnDelay)
        {
            int paternType = Random.Range(0, 3); //랜덤한 패턴을 실행
            switch (paternType)
            {
                case 0:
                    StartCoroutine(EnemyPatern0());
                    break;
                case 1:
                    StartCoroutine(EnemyPatern1());
                    break;
                case 2:
                    int randSpawn = Random.Range(0, 2) + 2;
                    StartCoroutine(EnemyPatern2(randSpawn));
                    break;
            }
            curSpawnelay = 0;
        }
    }

    public void RespawnPlayer() //플레이어가 죽었을 때 2초 후에 부활
    {
        Invoke("Respawn", 2f);
    }
    void Respawn()
    {
        player.transform.position = Vector3.down * 3.7f;
        player.SetActive(true);
    }

    IEnumerator EnemyPatern0() //1번째 적 비행기 패턴
    {
        SpawnEnemy0(1);
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < 3; i++)
        {
            // 적 비행기 생성 코드
            SpawnEnemy0(0);
            yield return new WaitForSeconds(0.2f);
        }
    }
    void SpawnEnemy0(int enemyType) //1번째 패턴 적 생성
    {
        GameObject enemy = Instantiate(enemyObjs[enemyType], spawnPoints[0].position, Quaternion.identity);
        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>(); //객체 생성 후 값 전달
        enemyLogic.player = player;
        enemyLogic.MovePaternPoints0 = MovePaternPoints0;
        enemyLogic.itemWayPoints = itemWayPoints;
        enemyLogic.paternType = 0;
    }
    IEnumerator EnemyPatern1() //2번째 적 비행기 패턴
    {
        SpawnEnemy1(1);
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < 3; i++)
        {
            // 적 비행기 생성 코드
            SpawnEnemy1(0);
            yield return new WaitForSeconds(0.2f);
        }
    }
    void SpawnEnemy1(int enemyType) //2번째 패턴 적 생성
    {
        GameObject enemy = Instantiate(enemyObjs[enemyType], spawnPoints[1].position, Quaternion.identity);
        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>(); //생성 후 값 전달
        enemyLogic.player = player;
        enemyLogic.MovePaternPoints1 = MovePaternPoints1;
        enemyLogic.itemWayPoints = itemWayPoints;
        enemyLogic.paternType = 1;
    }
    IEnumerator EnemyPatern2(int spawnPoint)
    {
        GameObject enemy = Instantiate(enemyObjs[2], spawnPoints[spawnPoint].position, Quaternion.identity);
        EnemyCtrl enemyLogic = enemy.GetComponent <EnemyCtrl>();
        enemyLogic.player = player;
        enemyLogic.itemWayPoints = itemWayPoints;
        enemyLogic.paternType = 2;
        yield return null;
    }
/*    void CreateEnemy(int a)
    {
        switch(a)
        {
            case 0:
                GameObject.Instantiate(enemy1);
                break;
            case 1:
                GameObject.Instantiate(enemy2);
                break;
            case 2:
                GameObject.Instantiate(enemy3);
                break;
        }
    }
    IEnumerator GenerateEnemy()
    {
        yield return new WaitForSeconds(2f);

        int numEnemy = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;
        Vector2 pos = new Vector2(Random.Range(-2.5f, 2.5f), 6.5f);
        int type = Random.Range(0, 6);
        if (numEnemy < maxEnemy)
        {
            if (type == 0)
            {
                Instantiate(enemy3, pos, Quaternion.identity);
            }
            else if (type == 1)
            {
                Instantiate(enemy2, pos, Quaternion.identity);
            }
            else
            {
                Instantiate(enemy1, pos, Quaternion.identity);
            }
        }
    }
*/}