using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public class EnemyCtrl : MonoBehaviour
{
    public string enemyName;
    public float speed = 5;
    public int hp;
    public Sprite[] sprites;
    public Transform[] wayPoints;

    public GameObject Bullet_0;
    public GameObject Bullet_1;
    public GameObject player;

    public float maxShotDelay;
    public float curShotDelay;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        StartCoroutine(BazierMove());
    }

    void OnHit(int dmg) //피격 시 실행되는 메소드
    {
        hp -= dmg;

        spriteRenderer.sprite = sprites[1]; //피격 이펙트
        Invoke("ReturnSprite", 0.1f); //0.1초 딜레이 후 원래대로

        if(hp <= 0)
        {
            Destroy(gameObject);
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

    void Fire()
    {
        curShotDelay += Time.deltaTime; //총알 딜레이
        if (curShotDelay > maxShotDelay) //일정 시간마다 발사
        {
            Vector3 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            if (enemyName == "B")
            {
                Instantiate(Bullet_0, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            }
            else if (enemyName == "C")
            {
                Instantiate(Bullet_1, transform.position + Vector3.right * 0.3f, Quaternion.AngleAxis(angle, Vector3.forward));
                Instantiate(Bullet_1, transform.position - Vector3.right * 0.3f, Quaternion.AngleAxis(angle, Vector3.forward));
            }
            curShotDelay = 0;
        }
    }

    IEnumerator BazierMove()
    {
        float t = 0f;
        while (true)
        {
            for (int i = 0; i < 5; i += 4)
            {
                while (t < 1f)
                {
                    transform.position = Mathf.Pow(1 - t, 3) * wayPoints[i].position
                            + 3 * t * Mathf.Pow(1 - t, 2) * wayPoints[i + 1].position
                            + 3 * Mathf.Pow(t, 2) * (1 - t) * wayPoints[i + 2].position
                            + Mathf.Pow(t, 3) * wayPoints[i + 3].position;

                    t += Time.deltaTime / 3.0f;
                    yield return null;
                }
                t = 0f;
            }
        }
    }
}
