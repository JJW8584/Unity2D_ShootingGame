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
    private int[] hpSet;

    public float maxSpawnDelay = 5f;
    private float curSpawnelay = 3f;

    public float ScoreDelay = 0.1f;
    private float curScoreDelay = 0.0f;

    public GameObject player;

    public TextMeshProUGUI scoreText; //UI
    public Image[] lifeImages;
    public Image[] boomImages;
    public GameObject gameOverSet;
    private bool isBossSpawn = false;

    public ObjectManager objectManager;

    private void Awake()
    {
        if (Instance == null) //�̱��� ������Ʈ ����
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
        //UI ������Ʈ
        PlayerCtrl playerLogic = player.GetComponent<PlayerCtrl>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);

        curSpawnelay += Time.deltaTime;
        curScoreDelay += Time.deltaTime;
        if (player.activeSelf)
            if(curScoreDelay >= ScoreDelay)
            {
                playerLogic.score += 1;
                curScoreDelay = 0;
            }
        if (playerLogic.score < 3000)
        {
            if (curSpawnelay >= maxSpawnDelay)
            {
                int paternType = Random.Range(0, 3); //������ ������ ����
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
        else if (!isBossSpawn)
        {
            SpawnBoss();
            isBossSpawn = true;
        }
    }

    public void RespawnPlayer() //�÷��̾ �׾��� �� 2�� �Ŀ� ��Ȱ
    {
        Invoke("Respawn", 2f);
    }
    void Respawn()
    {
        player.transform.position = Vector3.down * 3.7f;
        player.SetActive(true);

        PlayerCtrl playerLogic = player.GetComponent<PlayerCtrl>();
        playerLogic.isHit = false;
    }
    public void UpdateLifeIcon(int life)
    {
        for(int i = 0; i < 3; i++) //���� ��Ȱ��ȭ ��
        {
            lifeImages[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < life; i++) //�����ִ� ��ŭ�� Ȱ��ȭ
        {
            lifeImages[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int boom)
    {
        for (int i = 0; i < 3; i++) //���� ��Ȱ��ȭ ��
        {
            boomImages[i].color = new Color(1, 1, 1, 0);
        }

        for (int i = 0; i < boom; i++) //�����ִ� ��ŭ�� Ȱ��ȭ
        {
            boomImages[i].color = new Color(1, 1, 1, 1);
        }
    }
    public void GameOver()
    {
        for (int i = 0; i < 3; i++) //���� ��Ȱ��ȭ ��
        {
            lifeImages[i].color = new Color(1, 1, 1, 0);
        }
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
    IEnumerator EnemyPatern0() //1��° �� ����� ����
    {
        SpawnEnemy0(1);
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < 3; i++)
        {
            // �� ����� ���� �ڵ�
            SpawnEnemy0(0);
            yield return new WaitForSeconds(0.2f);
        }
    }
    void SpawnEnemy0(int enemyType) //1��° ���� �� ����
    {
        GameObject enemy = objectManager.MakeObj(enemyObj[enemyType]);
        enemy.transform.position = spawnPoints[0].position;

        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>(); //��ü ���� �� �� ����
        enemyLogic.player = player;
        enemyLogic.MovePaternPoints0 = MovePaternPoints0;
        enemyLogic.paternType = 0;
        enemyLogic.objectManager = objectManager;
        enemyLogic.hp = hpSet[enemyType] * player.GetComponent<PlayerCtrl>().power * 2;
    }
    IEnumerator EnemyPatern1() //2��° �� ����� ����
    {
        SpawnEnemy1(1);
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < 3; i++)
        {
            // �� ����� ���� �ڵ�
            SpawnEnemy1(0);
            yield return new WaitForSeconds(0.2f);
        }
    }
    void SpawnEnemy1(int enemyType) //2��° ���� �� ����
    {
        GameObject enemy = objectManager.MakeObj(enemyObj[enemyType]);
        enemy.transform.position = spawnPoints[1].position;

        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>(); //���� �� �� ����
        enemyLogic.player = player;
        enemyLogic.MovePaternPoints1 = MovePaternPoints1;
        enemyLogic.paternType = 1;
        enemyLogic.objectManager = objectManager;
        enemyLogic.hp = hpSet[enemyType] * player.GetComponent<PlayerCtrl>().power * 2;
    }
    IEnumerator EnemyPatern2(int spawnPoint) //3��° ����
    {
        GameObject enemy = objectManager.MakeObj(enemyObj[2]);
        enemy.transform.position = spawnPoints[spawnPoint].position;

        EnemyCtrl enemyLogic = enemy.GetComponent <EnemyCtrl>();
        enemyLogic.player = player;
        enemyLogic.paternType = 2;
        enemyLogic.objectManager = objectManager;
        enemyLogic.hp = hpSet[2] * player.GetComponent<PlayerCtrl>().power * 2;
        yield return null;
    }

    void SpawnBoss()
    {
        GameObject enemy = objectManager.MakeObj(enemyObj[3]);
        enemy.transform.position = spawnPoints[4].position;

        EnemyCtrl enemyLogic = enemy.GetComponent<EnemyCtrl>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;
    }
}