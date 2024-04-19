using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UnityEngine;
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

    [SerializeField] private int Type; //플레이어 방향으로 이동
    private Transform target;

    public GameObject Bullet_0;
    public GameObject Bullet_1;
    public GameObject player;

    Quaternion _rot;

    public float maxShotDelay;
    public float curShotDelay;

    SpriteRenderer spriteRenderer;
    Animator anim;

    public GameObject destructionEffectPrefab; // 파괴 이펙트 프리팹
    public GameObject[] itemPrefab; // 아이템 프리팹

    public int iter = 0;
    public Transform targetTr;
    [SerializeField] private int b_Type;
    float _angle;
    private float atkPoint;
    private bool itemSpawn;

    public ObjectManager objectManager;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(enemyName == "Boss")
            anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        /*atkPoint = Random.Range(2.0f, 4.0f);
        Type = Random.Range(0, 4) + 1;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyName == "Boss")
            return;
        FireToPlayer();
        switch (paternType)
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
        }
    }
    private void OnEnable()
    {
        itemSpawn = false;
    }

    public void OnHit(int dmg) //피격 시 실행되는 메소드
    {

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
            PlayerCtrl playerLogic = player.GetComponent<PlayerCtrl>();
            playerLogic.score += enemyScore;
            // 아이템 생성, 일정 확률로 아이템 생성
            if (itemPrefab != null && !itemSpawn)
            {
                int itemRand = Random.Range(0, 3);
                if (enemyName == "Boss")
                    itemRand = 1;
                if (enemyName != "A" && itemRand == 0)
                {
                    int randItemType = Random.Range(0, 10);
                    if (randItemType < 3)
                    {
                        GameObject itemObj = objectManager.MakeObj("itemPower");
                        itemObj.transform.position = transform.position;
                    }
                    else if (randItemType < 8)
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

    void MoveToPlayer() //플레이어에게 이동
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>(); //tag로 플레이어를 찾아 이동
        Vector2 atkDir = target.position - transform.position;
        transform.Translate(atkDir.normalized * speed * Time.deltaTime);
        _angle = Mathf.Atan2(atkDir.y, atkDir.x) * Mathf.Rad2Deg + 90; //플레이어를 향해서 회전
        _rot = Quaternion.Euler(0, 0, _angle);
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
}
