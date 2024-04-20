using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class EnemyCtrl : MonoBehaviour
{
    public string enemyName;
    public int enemyScore;
    public float speed = 5;
    public int hp;
    public int paternType;
    public Sprite[] sprites;
    public Transform[] MovePaternPoints0;
    public Transform[] MovePaternPoints1;
    public Transform[] bossWayPoints;

    [SerializeField] private int Type; //플레이어 방향으로 이동
    private Transform target;

    public GameObject Bullet_0;
    public GameObject Bullet_1;
    public GameObject player;

    Quaternion _rot;

    public float maxShotDelay;
    public float curShotDelay;
    public float maxPaternDelay = 5f;
    public float curPaternDelay = 0f;

    SpriteRenderer spriteRenderer;
    Animator anim;

    public GameObject destructionEffectPrefab; // 파괴 이펙트 프리팹
    public GameObject[] itemPrefab; // 아이템 프리팹

    public int iter = 0;
    public Transform targetTr;
    [SerializeField] private int b_Type;
    private float atkPoint;
    private bool itemSpawn;

    public int bossPatern = 0;
    public int curBossPatern = 0;
    public int[] maxBossPatern;
    private int bossBulletAng = 0;
    private bool bossOpening = true;
    private bool isPaternTime = true;
    private float stopTime = 0;

    public ObjectManager objectManager;
    public GameManager gameManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(enemyName == "Boss")
            anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaternTime && enemyName == "Boss") //패턴 간 딜레이를 주고 싶음...how?
            curPaternDelay += Time.deltaTime;
        if (curPaternDelay > maxPaternDelay)
            isPaternTime = true;

        if (enemyName != "Boss" && !gameManager.isBossSpawn)
        {
            FireToPlayer();
        }
        else if (!bossOpening && isPaternTime)
        {
            switch (bossPatern)
            {
                case 0:
                    maxShotDelay = 1.5f;
                    BossShot();
                    break;
                case 1:
                    maxShotDelay = 0.5f;
                    BossShotToPlayer();
                    break;
                case 2:
                    maxShotDelay = 0.1f;
                    BossShotArc();
                    break;
                case 3:
                    maxShotDelay = 1.5f;
                    BossShotCir();
                    break;
            }
        }

        switch (paternType) //enemy들이 움직이는 코드
        {
            case 0:
                StartCoroutine(BazierMove0()); //코루틴으로 실행
                break;
            case 1:
                StartCoroutine(BazierMove1());
                break;
            case 2:
                atkPoint = 2.5f;
                StartCoroutine(EnemyMovePatern0());
                break;
            case 3: //boss patern
                StartCoroutine(BossPatern());
                break;
        }
    }
    private void OnEnable()
    {
        itemSpawn = false;
    }

    public void OnHit(int dmg) //피격 시 실행되는 메소드
    {
        if (hp <= 0)
            return;

        hp -= dmg;

        if (enemyName == "Boss")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1]; //피격 이펙트
            Invoke("ReturnSprite", 0.05f); //딜레이 후 원래대로
        }

        if(hp <= 0)
        {
             gameManager.score += enemyScore;
            if (enemyName == "Boss")
            {
                PlayerPrefs.SetInt("score", gameManager.score);
                PlayerPrefs.SetFloat("playerX", player.transform.position.x);
                PlayerPrefs.SetFloat("playerY", player.transform.position.y);
                SceneManager.LoadScene(2);
            }
            // 아이템 생성, 일정 확률로 아이템 생성
            if (itemPrefab != null && !itemSpawn)
            {
                int itemRand = Random.Range(0, 20);
                if (enemyName == "Boss")
                    itemRand = 10;
                if (enemyName != "A" && itemRand < 10)
                {
                    if (itemRand < 4)
                    {
                        GameObject itemObj = objectManager.MakeObj("itemPower");
                        itemObj.transform.position = transform.position;
                    }
                    else if (itemRand < 8)
                    {
                        GameObject itemObj = objectManager.MakeObj("itemCoin"); 
                        itemObj.transform.position = transform.position;
                    }
                    else
                    {
                        GameObject itemObj = objectManager.MakeObj("itemBoom"); 
                        itemObj.transform.position = transform.position;
                    }
                }
                itemSpawn = false;
            }
            gameObject.SetActive(false);
            // 파괴 이펙트 생성
            GameObject destructionEffect = objectManager.MakeObj("destroyEffect");
            destructionEffect.transform.position = transform.position;
        }
    }
    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0]; //원래대로 돌아옴
    }

    private void OnTriggerEnter2D(Collider2D collision) //Collider값은 충돌 여부만 체크
    {
        if (collision.tag == "Bullet") //불릿과 충돌
        {
            BulletCtrl bulletCtrl = collision.GetComponent<BulletCtrl>();
            OnHit(bulletCtrl.dmg);
        }
    }

    void FireToPlayer() //플레이어에게 총알 발사
    {
        curShotDelay += Time.deltaTime; //총알 딜레이
        if (curShotDelay > maxShotDelay) //일정 시간마다 발사
        {
            Vector3 dir = player.transform.position - transform.position; //총알을 플레이어 방향으로 회전
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            GameObject bulletObj;
            if (enemyName == "B")
            {
                bulletObj = objectManager.MakeObj("enemyBullet0");
                bulletObj.transform.SetPositionAndRotation(transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            }
            else if (enemyName == "C")
            {
                bulletObj = objectManager.MakeObj("enemyBullet1");
                bulletObj.transform.SetPositionAndRotation(transform.position + Vector3.right * 0.3f, Quaternion.AngleAxis(angle, Vector3.forward));
                bulletObj = objectManager.MakeObj("enemyBullet1");
                bulletObj.transform.SetPositionAndRotation(transform.position - Vector3.right * 0.3f, Quaternion.AngleAxis(angle, Vector3.forward));
            }
            curShotDelay = Random.Range(0f, 0.7f);
        }
    }

    IEnumerator BazierMove0()
    {
        float t = 0f;
        while (true)
        {
            for (int i = 0; i < 5; i += 4)
            {
                while (t < 1f)
                {
                    transform.position = Mathf.Pow(1 - t, 3) * MovePaternPoints0[i].position  //베지어 곡선을 따라 이동
                            + 3 * t * Mathf.Pow(1 - t, 2) * MovePaternPoints0[i + 1].position
                            + 3 * Mathf.Pow(t, 2) * (1 - t) * MovePaternPoints0[i + 2].position
                            + Mathf.Pow(t, 3) * MovePaternPoints0[i + 3].position;

                    Vector2 tangent = CalculateBezierTangent0(t, i); //접선 방향으로 회전
                    float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg + 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    t += Time.deltaTime / 2.5f;

                    yield return null;
                }
                t = 0f;
            }
        }
    }
    Vector2 CalculateBezierTangent0(float t, int i) //베지어 곡선 접선 계산
    {
        Vector2 tangent = -3 * Mathf.Pow(1 - t, 2) * (Vector2)MovePaternPoints0[i].position;
        tangent += (3 * Mathf.Pow(1 - t, 2) - 6 * t * (1 - t)) * (Vector2)MovePaternPoints0[i + 1].position;
        tangent += (-3 * Mathf.Pow(t, 2) + 6 * t * (1 - t)) * (Vector2)MovePaternPoints0[i + 2].position;
        tangent += 3 * Mathf.Pow(t, 2) * (Vector2)MovePaternPoints0[i + 3].position;

        return tangent.normalized;
    }
    IEnumerator BazierMove1()
    {
        float t = 0f;
        while (true)
        {
            for (int i = 0; i < 5; i += 4)
            {
                while (t < 1f)
                {
                    transform.position = Mathf.Pow(1 - t, 3) * MovePaternPoints1[i].position  //베지어 곡선을 따라 이동
                            + 3 * t * Mathf.Pow(1 - t, 2) * MovePaternPoints1[i + 1].position
                            + 3 * Mathf.Pow(t, 2) * (1 - t) * MovePaternPoints1[i + 2].position
                            + Mathf.Pow(t, 3) * MovePaternPoints1[i + 3].position;

                    Vector2 tangent = CalculateBezierTangent1(t, i); //접선 방향으로 회전
                    float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg + 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    t += Time.deltaTime / 2.5f;

                    yield return null;
                }
                t = 0f;
            }
        }
    }
    Vector2 CalculateBezierTangent1(float t, int i) //베지어 곡선 접선 계산
    {
        Vector2 tangent = -3 * Mathf.Pow(1 - t, 2) * (Vector2)MovePaternPoints1[i].position;
        tangent += (3 * Mathf.Pow(1 - t, 2) - 6 * t * (1 - t)) * (Vector2)MovePaternPoints1[i + 1].position;
        tangent += (-3 * Mathf.Pow(t, 2) + 6 * t * (1 - t)) * (Vector2)MovePaternPoints1[i + 2].position;
        tangent += 3 * Mathf.Pow(t, 2) * (Vector2)MovePaternPoints1[i + 3].position;

        return tangent.normalized;
    }
    IEnumerator EnemyMovePatern0()
    {
        if (transform.position.y < atkPoint)
        {
            FireToPlayer();
        }
        else
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        yield return null;
    }

    IEnumerator BossPatern()
    {
        if ((transform.position.y > 2.5f) && bossOpening)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            if (transform.position.y <= 2.5f)
                bossOpening = false;
        }
        else if (stopTime > 1f)
        {
            float t = 0f;
            while (true)
            {
                //bossWayPoints[1].transform.position = new Vector3(4f, Random.Range(1f, 4f));
                //bossWayPoints[2].transform.position = new Vector3(-4f, Random.Range(1f, 4f));
                while (t < 1f)
                {
                    transform.position = Mathf.Pow(1 - t, 3) * bossWayPoints[0].position  //베지어 곡선을 따라 이동
                            + 3 * t * Mathf.Pow(1 - t, 2) * bossWayPoints[1].position
                            + 3 * Mathf.Pow(t, 2) * (1 - t) * bossWayPoints[2].position
                            + Mathf.Pow(t, 3) * bossWayPoints[3].position;
                    t += Time.deltaTime / 5f;
                    yield return null;
                }
                t = 0f;
                yield return null;
            }
        }
        else
            stopTime += Time.deltaTime;
        yield return null;
    }
    
    void BossShot()
    {
        curShotDelay += Time.deltaTime;
        if (curShotDelay > maxShotDelay)
        {
            for (int i = 0; i < 4; ++i) //집중탄
            {
                GameObject bulletObj = objectManager.MakeObj("bossBullet0");
                bulletObj.transform.position = transform.position + new Vector3(((1.0f - 4) / 2.0f + i) * 0.8f, -1.2f, 0f);
                bulletObj.transform.rotation = Quaternion.identity;
            }
            ++curBossPatern;
            curShotDelay = 0f;
        }
        if (curBossPatern >= maxBossPatern[bossPatern])
        {
            isPaternTime = false;
            curPaternDelay = 0;
            curBossPatern = 0;
            bossPatern = bossPatern == 3 ? 0 : ++bossPatern;
        }
    }
    void BossShotToPlayer()
    {
        curShotDelay += Time.deltaTime; //총알 딜레이
        if (curShotDelay > maxShotDelay) //일정 시간마다 발사
        {
            Vector3 dir = player.transform.position - transform.position; //총알을 플레이어 방향으로 회전
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
            GameObject bulletObj = objectManager.MakeObj("bossBullet0");
            bulletObj.transform.SetPositionAndRotation(transform.position + new Vector3(0f, -1.2f), Quaternion.AngleAxis(angle, Vector3.forward));
            ++curBossPatern;
            curShotDelay = 0;
        }
        if (curBossPatern >= maxBossPatern[bossPatern])
        {
            isPaternTime = false;
            curPaternDelay = 0;
            curBossPatern = 0;
            bossPatern = bossPatern == 3 ? 0 : ++bossPatern;
        }
    }
    void BossShotArc()
    {
        curShotDelay += Time.deltaTime; //총알 딜레이
        if (curShotDelay > maxShotDelay) //일정 시간마다 발사
        {
            GameObject bulletObj = objectManager.MakeObj("bossBullet1");
            bulletObj.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, (bossBulletAng++ % 10) * 10f - 45f));
            ++curBossPatern;
            curShotDelay = 0;
        }

        if (curBossPatern >= maxBossPatern[bossPatern])
        {
            bossBulletAng = 0;
            isPaternTime = false;
            curPaternDelay = 0;
            curBossPatern = 0;
            bossPatern = bossPatern == 3 ? 0 : ++bossPatern;
        }
    }
    void BossShotCir()
    {
        curShotDelay += Time.deltaTime; //총알 딜레이
        if (curShotDelay > maxShotDelay) //일정 시간마다 발사
        {
            for (int i = 0; i < 36; i++)
            {
                GameObject bulletObj = objectManager.MakeObj("bossBullet1");
                bulletObj.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0f, 0f, i * 10f));
            }
            ++curBossPatern;
            curShotDelay = 0;
        }

        if (curBossPatern >= maxBossPatern[bossPatern])
        {
            isPaternTime = false;
            curPaternDelay = 0;
            curBossPatern = 0;
            bossPatern = bossPatern == 3 ? 0 : ++bossPatern;
        }
    }
}
