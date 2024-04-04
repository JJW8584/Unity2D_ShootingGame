using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    public Transform[] MovePaternPoints0;
    public Transform[] MovePaternPoints1;
    public Transform[] itemWayPoints;

    public float maxSpawnDelay = 5f;
    private float curSpawnelay = 3f;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        curSpawnelay += Time.deltaTime;

        if (curSpawnelay >= maxSpawnDelay)
        {
            int paternType = Random.Range(0, 2); //랜덤한 패턴을 실행
            switch(paternType)
            {
                case 0:
                    StartCoroutine(EnemyPatern0());
                    break;
                case 1:
                    StartCoroutine(EnemyPatern1());
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
    void SpawnEnemy0(int enemyType)
    {
        GameObject enemy = Instantiate(enemyObjs[enemyType], spawnPoints[0].position, Quaternion.identity);
        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>();
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
    void SpawnEnemy1(int enemyType)
    {
        GameObject enemy = Instantiate(enemyObjs[enemyType], spawnPoints[1].position, Quaternion.identity);
        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>();
        enemyLogic.player = player;
        enemyLogic.MovePaternPoints1 = MovePaternPoints1;
        enemyLogic.itemWayPoints = itemWayPoints;
        enemyLogic.paternType = 1;
    }
}