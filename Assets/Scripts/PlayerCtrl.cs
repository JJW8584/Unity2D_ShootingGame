using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public int speed = 10;
    public int power = 1; //3������ �ִ�
    public int boom = 0; //3���� �ִ�
    public int life = 3;
    public int score = 0;
    public bool isHit;
    public bool isBoomTime = false;

    public GameObject Bullet_0;
    public GameObject Bullet_1;
    public Transform firePos;

    private int BulletCnt = 0;
    private float iter = 0;
    private Animator _animator;

    public GameManager gameManager;
    public GameObject destructionEffectPrefab; // �ı� ����Ʈ ������
    public GameObject boomEffect;

    //public Transform myTr;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        /*Debug.Log("Log");
        Debug.LogWarning("Warning");
        Debug.LogError("Error");*/
        //myTr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        Fire();
        Boom();
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal"); //x��
        float v = Input.GetAxis("Vertical"); //y��

        _animator.SetFloat("p_Hori", h); //�÷��̾��� p_Hori���� set, �ִϸ��̼��� ����

        Vector3 moveDir = Vector3.up * v + Vector3.right * h; //���⺤�� ��ü ����

        transform.Translate(moveDir * speed * Time.deltaTime); //�÷��̾��� �̵�

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); //ĳ������ ���� ��ǥ�� ����Ʈ ��ǥ��� ��ȯ
        viewPos.x = Mathf.Clamp01(viewPos.x); //x���� 0�̻�, 1���Ϸ� ����
        viewPos.y = Mathf.Clamp01(viewPos.y); //y���� 0�̻�, 1���Ϸ� ����
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos); //�ٽ� ���� ��ǥ�� ��ȯ
        transform.position = worldPos; //��ǥ�� ����
    }

    void Fire()
    {
        if (Input.GetMouseButton(0))
        {
            iter += Time.deltaTime; //�Ѿ� ������
            if (iter > 0.1) //���� �ð����� �߻�
            {
                power = power > 4 ? 4 : power;
                switch (power)
                {
                    case 1:
                        Instantiate(Bullet_0, firePos.position, Quaternion.identity);
                        break;
                    case 2:
                        for (int i = 0; i < 2; ++i) //����ź
                        {
                            Instantiate(Bullet_0, firePos.position + new Vector3(((1.0f - 2) / 2.0f + i) * 0.3f, 0f, 0f), Quaternion.identity);
                            //�÷��̾ ���� �������� ������ ����� �Ѿ� �� �߻�(���� ���� ����), ������ġ ����, y, z���� ��ȭ ����
                        }
                        break;
                    case 3:
                        Instantiate(Bullet_1, firePos.position, Quaternion.identity);
                        for (int i = 0; i < 2; ++i) //����ź
                        {
                            Instantiate(Bullet_0, firePos.position + new Vector3(((1.0f - 2) / 2.0f + i) * 0.65f, -0.1f, 0f), Quaternion.identity);
                            //�÷��̾ ���� �������� ������ ����� �Ѿ� �� �߻�(���� ���� ����), ������ġ ����
                        }
                        break;
                    case 4:
                        if (BulletCnt++ == 0)
                        {
                            for (int i = 0; i < 6; ++i) //Ȯ��ź
                            {
                                Instantiate(Bullet_1, firePos.position, Quaternion.Euler(0f, 0f, ((1.0f - 6) / 2.0f + i) * 5.0f));
                                //�Ѿ� ������ ���� ����, ������ z���� ȸ��, Quaternion ���Ϸ� ��
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 6; ++i) //Ȯ��ź
                            {
                                Instantiate(Bullet_0, firePos.position, Quaternion.Euler(0f, 0f, ((1.0f - 6) / 2.0f + i) * 5.0f));
                                //�Ѿ� ������ ���� ����, ������ z���� ȸ��, Quaternion ���Ϸ� ��
                            }
                        }
                        BulletCnt = BulletCnt > 5 ? 0 : BulletCnt;
                        break;
                }
            }
            iter = iter > 0.1 ? 0 : iter; //�߻� �� ������ �ʱ�ȭ
        }
    }

    void Boom()
    {
        if(Input.GetMouseButton(1))
        {
            if (boom > 0)
            {
                if (!isBoomTime)
                {
                    --boom;
                    gameManager.UpdateBoomIcon(boom);
                    isBoomTime = true;
                    boomEffect.SetActive(true); //�ı� ����Ʈ ����
                    Invoke("BoomEffectOff", 2f);

                    GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
                    for (int i = 0; i < enemys.Length; i++) //�� ��ü �ı�
                    {
                        EnemyCtrl enemyLogic = enemys[i].GetComponent<EnemyCtrl>();
                        enemyLogic.OnHit(100);
                    }
                    GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("Enemy Bullet");
                    for (int i = 0; i < enemys.Length; i++) //�� �Ѿ�
                    {
                        Destroy(enemyBullets[i]);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Enemy Bullet") //�� �Ѿ˿� �¾��� �� ����
        {
            if (isHit)
                return;

            isHit = true;

            life--;
            if(life <= 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.UpdateLifeIcon(life);
            }
            gameObject.SetActive(false); //��Ȱ��ȭ
            if (destructionEffectPrefab != null) // �ı� ����Ʈ ����
            {
                Instantiate(destructionEffectPrefab, transform.position, Quaternion.identity);
            }
            gameManager.RespawnPlayer(); //2�� �� ��Ȱ
        }
        else if(collision.tag == "PowerItem") //�������� �Ծ��� ��
        {
            ++power;
            score += 30;
        }
        else if(collision.tag == "BoomItem")
        {
            boom = boom < 3 ? ++boom : 3;
            gameManager.UpdateBoomIcon(boom);
            score += 50;
        }
        else if(collision.tag == "Coin")
        {
            score += 500;
        }
    }
    void BoomEffectOff()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }
}