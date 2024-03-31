using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyCtrl : MonoBehaviour
{
    public string enemyName;
    public float speed = 5;
    public int hp;
    public Sprite[] sprites;

    public GameObject Bullet_0;
    public GameObject Bullet_1;

    public float t = 0f;

    private float iter = 0;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.down * speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (collision.tag == "Bullet") //불릿과 충돌하면 로그 메시지 출력
        {
            BulletCtrl bulletCtrl = collision.GetComponent<BulletCtrl>();
            OnHit(bulletCtrl.dmg);
        }
    }

    void Fire()
    {
        iter += Time.deltaTime; //총알 딜레이
        if (iter > 0.5) //일정 시간마다 발사
        {
            if (enemyName == "Enemy B")
            {
                GameObject bullet = Instantiate(Bullet_0, transform.position, Quaternion.identity);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            }
            else if (enemyName == "Enemy C")
            {

            }
        }
        iter = iter > 0.5 ? 0 : iter; //발사 후 딜레이 초기화
    }
}
