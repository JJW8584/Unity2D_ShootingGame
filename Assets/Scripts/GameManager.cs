using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjs;
    public Transform[] spawnPoints;

    public float maxSpawnDelay;
    public float curSpawnelay;

    // Start is called before the first frame update
    void Start()
    {
        
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
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        int ranPatern = Random.Range(0, 3);
        EnemyPatern(0);
    }

    void EnemyPatern(int num)
    {
        switch (num)
        {
            case 0:
                break;
        }
    }
    
}
