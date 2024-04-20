using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject[] enemyObjs;
    public string[] enemyObj;
    public Transform[] spawnPoints;
    public Transform[] MovePaternPoints0;
    public Transform[] MovePaternPoints1;
    public Transform[] bossWayPoints;
    private int[] hpSet;

    public float maxSpawnDelay = 5f;
    private float curSpawnelay = 3f;

    public int score = 0;
    public float ScoreDelay = 0.1f;
    private float curScoreDelay = 0.0f;

    public GameObject player;

    public TextMeshProUGUI scoreText; //UI
    public Image[] lifeImages;
    public Image[] boomImages;
    public GameObject gameOverSet;
    public GameObject[] gamePlaySet;

    public bool isBossSpawn = false;
    public bool gameOver = false;

    public ObjectManager objectManager;

    private void Awake()
    {
        if (Instance == null) //싱글턴 오브젝트 생성
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        enemyObj = new string[] { "enemyA", "enemyB", "enemyC", "boss" };
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateBoomIcon(0);
        hpSet = new int[] { 1, 6, 17 };
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) //게임이 실행 중일 때
        {
            //UI 업데이트
            scoreText.text = string.Format("{0:n0}", score); //형식 지정

            curSpawnelay += Time.deltaTime; //적을 생성할 딜레이
            curScoreDelay += Time.deltaTime; //일정 시간마다 점수 추가(생존 점수)
            if (player.activeSelf) //플레이어가 살아있을 때만 생존 점수 추가
                if (curScoreDelay >= ScoreDelay)
                {
                    score += 1;
                    curScoreDelay = 0;
                }
            if (score < 4000) //4000점이 넘으면 보스 소환
            {
                if (curSpawnelay >= maxSpawnDelay) //일정 시간마다 실행
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
            else if (!isBossSpawn) //보스가 소환되지 않았을 때
            {
                maxSpawnDelay = 7f; //적이 출현할 딜레이 조정
                StartCoroutine(SpawnBoss()); //보스 소환
                isBossSpawn = true;
            }
            if (isBossSpawn) //보스 소환이 끝났을 때
            {
                if (curSpawnelay >= maxSpawnDelay)
                {
                    int paternType = Random.Range(0, 2); //랜덤한 패턴을 실행
                    switch (paternType) //총알 발사x 적 생성
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
        }
    }

    public void RespawnPlayer() //플레이어가 죽었을 때 2초 후에 부활
    {
        if (player.GetComponent<PlayerCtrl>().life > 0)
            Invoke("Respawn", 2f);
    }
    void Respawn()
    {
        player.transform.position = Vector3.down * 3.7f;
        player.SetActive(true);

        PlayerCtrl playerLogic = player.GetComponent<PlayerCtrl>();
        playerLogic.HP.transform.localScale = new Vector3(1f, 1f, 0);
        playerLogic.HP.GetComponent<SpriteRenderer>().color = Color.green;
        playerLogic.isHit = false;

        Invoke("PlayerInvincible", 2f);
    }
    void PlayerInvincible()
    {
        player.GetComponent<PlayerCtrl>().Invincible = false;
    }
    public void UpdateLifeIcon(int life)
    {
        for(int i = 0; i < 3; i++) //생명 비활성화 후
        {
            lifeImages[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < life; i++) //남아있는 만큼만 활성화
        {
            lifeImages[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int boom)
    {
        for (int i = 0; i < 3; i++) //생명 비활성화 후
        {
            boomImages[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < boom; i++) //남아있는 만큼만 활성화
        {
            boomImages[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void GameOver()
    {
        for (int i = 0; i < 3; i++) //생명 비활성화 후
        {
            lifeImages[i].color = new Color(1, 1, 1, 0);
        }
        gameOver = true; //게임 오버 상태

        //적 개체 파괴
        GameObject[] enemyA = objectManager.GetPool("enemyA");
        GameObject[] enemyB = objectManager.GetPool("enemyB");
        GameObject[] enemyC = objectManager.GetPool("enemyC");
        GameObject[] boss = objectManager.GetPool("boss");
        for (int i = 0; i < enemyA.Length; i++)
        {
            if (enemyA[i].activeSelf)
            {
                EnemyCtrl enemyLogic = enemyA[i].GetComponent<EnemyCtrl>();
                enemyLogic.OnHit(100);
            }
        }
        for (int i = 0; i < enemyB.Length; i++)
        {
            if (enemyB[i].activeSelf)
            {
                EnemyCtrl enemyLogic = enemyB[i].GetComponent<EnemyCtrl>();
                enemyLogic.OnHit(100);
            }
        }
        for (int i = 0; i < enemyC.Length; i++)
        {
            if (enemyC[i].activeSelf)
            {
                EnemyCtrl enemyLogic = enemyC[i].GetComponent<EnemyCtrl>();
                enemyLogic.OnHit(100);
            }
        }
        for (int i = 0; i < boss.Length; i++)
        {
            if (boss[i].activeSelf)
            {
                EnemyCtrl enemyLogic = boss[i].GetComponent<EnemyCtrl>();
                enemyLogic.OnHit(500);
            }
        }

        //적 총알 파괴
        GameObject[] enemyBullets0 = objectManager.GetPool("enemyBullet0");
        GameObject[] enemyBullets1 = objectManager.GetPool("enemyBullet1");
        GameObject[] bossBullets0 = objectManager.GetPool("bossBullet0");
        GameObject[] bossBullets1 = objectManager.GetPool("bossBullet1");
        for (int i = 0; i < enemyBullets0.Length; i++)
        {
            if (enemyBullets0[i].activeSelf)
            {
                enemyBullets0[i].SetActive(false);
            }
        }
        for (int i = 0; i < enemyBullets1.Length; i++)
        {
            if (enemyBullets1[i].activeSelf)
            {
                enemyBullets0[i].SetActive(false);
            }
        }
        for (int i = 0; i < bossBullets0.Length; i++)
        {
            if (bossBullets0[i].activeSelf)
            {
                bossBullets0[i].SetActive(false);
            }
        }
        for (int i = 0; i < bossBullets1.Length; i++)
        {
            if (bossBullets1[i].activeSelf)
            {
                bossBullets1[i].SetActive(false);
            }
        }
        for(int i = 0; i < gamePlaySet.Length; i++)
        {
            gamePlaySet[i].SetActive(false);
        }
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(1);
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
        GameObject enemy = objectManager.MakeObj(enemyObj[enemyType]);
        enemy.transform.position = spawnPoints[0].position;

        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>(); //객체 생성 후 값 전달
        enemyLogic.player = player;
        enemyLogic.MovePaternPoints0 = MovePaternPoints0;
        enemyLogic.paternType = 0;
        enemyLogic.objectManager = objectManager;
        enemyLogic.gameManager = this;
        enemyLogic.hp = hpSet[enemyType] * player.GetComponent<PlayerCtrl>().power * 2;
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
        GameObject enemy = objectManager.MakeObj(enemyObj[enemyType]);
        enemy.transform.position = spawnPoints[1].position;

        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>(); //생성 후 값 전달
        enemyLogic.player = player;
        enemyLogic.MovePaternPoints1 = MovePaternPoints1;
        enemyLogic.paternType = 1;
        enemyLogic.objectManager = objectManager;
        enemyLogic.gameManager = this;
        enemyLogic.hp = hpSet[enemyType] * player.GetComponent<PlayerCtrl>().power * 2;
    }
    IEnumerator EnemyPatern2(int spawnPoint) //3번째 패턴
    {
        GameObject enemy = objectManager.MakeObj(enemyObj[2]);
        enemy.transform.position = spawnPoints[spawnPoint].position;

        EnemyCtrl enemyLogic = enemy.GetComponent <EnemyCtrl>();
        enemyLogic.player = player;
        enemyLogic.paternType = 2;
        enemyLogic.objectManager = objectManager;
        enemyLogic.gameManager = this;
        enemyLogic.hp = hpSet[2] * player.GetComponent<PlayerCtrl>().power * 2;
        yield return null;
    }

    IEnumerator SpawnBoss()
    {
        GameObject enemy = objectManager.MakeObj(enemyObj[3]);
        enemy.transform.position = spawnPoints[4].position;

        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;
        enemyLogic.gameManager = this;
        enemyLogic.paternType = 3;
        enemyLogic.bossWayPoints = bossWayPoints;
        yield return null;
    }
}