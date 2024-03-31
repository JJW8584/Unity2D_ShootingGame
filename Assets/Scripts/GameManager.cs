using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;
    public Transform[] wayPoints;

    public float maxSpawnDelay;
    public float curSpawnelay;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        /*  curSpawnelay = Time.deltaTime;

          if(curSpawnelay > maxSpawnDelay)
          {
              maxSpawnDelay = Random.Range(0.5f, 2f);
              curSpawnelay = 0;
          }*/
    }

    void SpawnEnemy()
    {
        int ranEnemy = Random.Range(0, 3);

        GameObject enemy = Instantiate(enemyObjs[1], spawnPoints[0].position, Quaternion.identity);
        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>();
        enemyLogic.player = player;
        enemyLogic.wayPoints = wayPoints;
    }

    public void RespawnPlayer()
    {
        Invoke("Respawn", 2f);
    }
    void Respawn()
    {
        player.transform.position = Vector3.down * 3.7f;
        player.SetActive(true);
    }
}
