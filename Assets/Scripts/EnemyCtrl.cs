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
        BazierMove();
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
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
        if (collision.tag == "Bullet") //�Ҹ��� �浹
        {
            BulletCtrl bulletCtrl = collision.GetComponent<BulletCtrl>();
            OnHit(bulletCtrl.dmg);
        }
    }

    void Fire()
    {
        curShotDelay += Time.deltaTime; //�Ѿ� ������
        if (curShotDelay > maxShotDelay) //���� �ð����� �߻�
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

    void BazierMove()
    {
        for (float t = 0; t < 1; t += Time.deltaTime * speed)
        {
            transform.position = Mathf.Pow(1 - t, 3) * wayPoints[0].position
                    + 3 * t * Mathf.Pow(1 - t, 2) * wayPoints[1].position
                    + 3 * Mathf.Pow(t, 2) * (1 - t) * wayPoints[2].position
                    + Mathf.Pow(t, 3) * wayPoints[3].position;
        }
    }
}
