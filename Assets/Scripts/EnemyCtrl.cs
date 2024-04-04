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
    public float speed = 5;
    public int hp;
    public int paternType;
    public Sprite[] sprites;
    public Transform[] MovePaternPoints0;
    public Transform[] MovePaternPoints1;
    public Transform[] itemWayPoints;

    [SerializeField] private int Type; //�÷��̾� �������� �̵�
    private Transform target;
    private float atkPoint;

    public GameObject Bullet_0;
    public GameObject Bullet_1;
    public GameObject player;

    public float maxShotDelay;
    public float curShotDelay;

    SpriteRenderer spriteRenderer;

    public GameObject destructionEffectPrefab; // �ı� ����Ʈ ������
    public GameObject itemPrefab; // ������ ������

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //atkPoint = Random.Range(2.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {

/*      iter++;

        if(transform.position.y > atkPoint)
        {
            switch (Type)
            {
                case 1:
                    transform.Translate(Vector2.down * speed * Time.deltaTime, Space.World);
                    break;
                case 2:
                    transform.rotation = _rot;
                    transform.Translate(Vector2.down * speed * Time.deltaTime, Space.World);
                    break;
                case 3:
                    transform.Translate(Vector2.down * speed * Time.deltaTime, Space.World);
                    break;
                default:
                    break;

            }
        }
        else
        {
            if(iter / 120 > 5)
            {
                switch (Type)
                {
                    case 1:
                        Instantiate(Bullet_0, transform.position, Quaternion.Euler(0, 0, 180));
                        break;
                    case 2:
                        Instantiate(Bullet_0, transform.position, Quaternion.Euler(0, 0, 180));
                        break;
                    case 3:
                        Instantiate(Bullet_0, transform.position, Quaternion.Euler(0, 0, 180));
                        break;
                }
            }
        }*/
        
        FireToPlayer();
        switch (paternType)
        {
            case 0:
                StartCoroutine(BazierMove0()); //�ڷ�ƾ���� ����
                break;
            case 1:
                StartCoroutine(BazierMove1());
                break;
        }
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

    void MoveToPlayer() //�÷��̾�� �̵�
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>(); //tag�� �÷��̾ ã�� �̵�
        Vector2 atkDir = target.position - transform.position;
        transform.Translate(atkDir.normalized * speed * Time.deltaTime);
        float _angle = Mathf.Atan2(atkDir.y, atkDir.x) * Mathf.Rad2Deg + 90; //�÷��̾ ���ؼ� ȸ��
        Quaternion _rot = Quaternion.Euler(0, 0, _angle);
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

    void FireToPlayer() //�÷��̾�� �Ѿ� �߻�
    {
        curShotDelay += Time.deltaTime; //�Ѿ� ������
        if (curShotDelay > maxShotDelay) //���� �ð����� �߻�
        {
            Vector3 dir = player.transform.position - transform.position; //�Ѿ��� �÷��̾� �������� ȸ��
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

    IEnumerator BazierMove0()
    {
        float t = 0f;
        while (true)
        {
            for (int i = 0; i < 5; i += 4)
            {
                while (t < 1f)
                {
                    transform.position = Mathf.Pow(1 - t, 3) * MovePaternPoints0[i].position  //������ ��� ���� �̵�
                            + 3 * t * Mathf.Pow(1 - t, 2) * MovePaternPoints0[i + 1].position
                            + 3 * Mathf.Pow(t, 2) * (1 - t) * MovePaternPoints0[i + 2].position
                            + Mathf.Pow(t, 3) * MovePaternPoints0[i + 3].position;

                    Vector2 tangent = CalculateBezierTangent0(t, i); //���� �������� ȸ��
                    float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg + 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    t += Time.deltaTime / 2.5f;

                    yield return null;
                }
                t = 0f;
            }
        }
    }
    Vector2 CalculateBezierTangent0(float t, int i) //������ � ���� ���
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
                    transform.position = Mathf.Pow(1 - t, 3) * MovePaternPoints1[i].position  //������ ��� ���� �̵�
                            + 3 * t * Mathf.Pow(1 - t, 2) * MovePaternPoints1[i + 1].position
                            + 3 * Mathf.Pow(t, 2) * (1 - t) * MovePaternPoints1[i + 2].position
                            + Mathf.Pow(t, 3) * MovePaternPoints1[i + 3].position;

                    Vector2 tangent = CalculateBezierTangent1(t, i); //���� �������� ȸ��
                    float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg + 90;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    t += Time.deltaTime / 2.5f;

                    yield return null;
                }
                t = 0f;
            }
        }
    }
    Vector2 CalculateBezierTangent1(float t, int i) //������ � ���� ���
    {
        Vector2 tangent = -3 * Mathf.Pow(1 - t, 2) * (Vector2)MovePaternPoints1[i].position;
        tangent += (3 * Mathf.Pow(1 - t, 2) - 6 * t * (1 - t)) * (Vector2)MovePaternPoints1[i + 1].position;
        tangent += (-3 * Mathf.Pow(t, 2) + 6 * t * (1 - t)) * (Vector2)MovePaternPoints1[i + 2].position;
        tangent += 3 * Mathf.Pow(t, 2) * (Vector2)MovePaternPoints1[i + 3].position;

        return tangent.normalized;
    }
    private void OnDestroy()
    {
        // �ı� ����Ʈ ����
        if (destructionEffectPrefab != null)
        {
            Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
        }

        // ������ ����, ����Ȯ���� ������ ����ϵ��� �߰� �ʿ�
        if (itemPrefab != null)
        {
            GameObject itemObj = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            ItemCtrl itemControl = itemObj.GetComponent<ItemCtrl>();
            itemControl.wayPoints = itemWayPoints;
        }
    }
}
