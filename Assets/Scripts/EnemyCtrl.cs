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

    void OnHit(int dmg) //�ǰ� �� ����Ǵ� �޼ҵ�
    {
        hp -= dmg;

        spriteRenderer.sprite = sprites[1]; //�ǰ� ����Ʈ
        Invoke("ReturnSprite", 0.1f); //0.1�� ������ �� �������

        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0]; //������� ���ƿ�
    }

    private void OnTriggerEnter2D(Collider2D collision) //Collider���� �浹 ���θ� üũ
    {
        if (collision.tag == "Bullet") //�Ҹ��� �浹�ϸ� �α� �޽��� ���
        {
            BulletCtrl bulletCtrl = collision.GetComponent<BulletCtrl>();
            OnHit(bulletCtrl.dmg);
        }
    }

    void Fire()
    {
        iter += Time.deltaTime; //�Ѿ� ������
        if (iter > 0.5) //���� �ð����� �߻�
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
        iter = iter > 0.5 ? 0 : iter; //�߻� �� ������ �ʱ�ȭ
    }
}
