using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public int speed = 10;
    public int power = 1; //3레벨이 최대
    public int boom = 0; //3개가 최대
    public int life = 3;
    public bool isHit;
    public bool isBoomTime = false;

    public Transform firePos;

    private int BulletCnt = 0;
    private float iter = 0;
    private float powerIter = 0;
    private Animator _animator;

    public GameManager gameManager;
    public GameObject destructionEffectPrefab; // 파괴 이펙트 프리팹
    public GameObject boomEffect;
    public GameObject[] followers;

    public ObjectManager objectManager;

    //public Transform myTr;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
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
        float h = Input.GetAxis("Horizontal"); //x값
        float v = Input.GetAxis("Vertical"); //y값

        _animator.SetFloat("p_Hori", h); //플레이어의 p_Hori값을 set, 애니메이션을 위함

        Vector3 moveDir = Vector3.up * v + Vector3.right * h; //방향벡터 객체 선언

        transform.Translate(moveDir * speed * Time.deltaTime); //플레이어의 이동

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); //캐릭터의 월드 좌표를 뷰포트 좌표계로 변환
        viewPos.x = Mathf.Clamp01(viewPos.x); //x값을 0이상, 1이하로 제한
        viewPos.y = Mathf.Clamp01(viewPos.y); //y값을 0이상, 1이하로 제한
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos); //다시 월드 좌표로 변환
        transform.position = worldPos; //좌표를 적용
    }

    void Fire()
    {
        iter += Time.deltaTime; //총알 딜레이
        if (power > 3)
            powerIter += Time.deltaTime;
        if (iter > 0.1) //일정 시간마다 발사
        {
            GameObject bulletObj;
            switch (power)
            {
                case 1:
                    bulletObj = objectManager.MakeObj("playerBullet0");
                    bulletObj.transform.position = firePos.position;
                    break;
                case 2:
                    for (int i = 0; i < 2; ++i) //집중탄
                    {
                        bulletObj = objectManager.MakeObj("playerBullet0");
                        bulletObj.transform.position = firePos.position + new Vector3(((1.0f - 2) / 2.0f + i) * 0.3f, 0f, 0f);
                        //플레이어가 보는 방향으로 레벨에 비례한 총알 수 발사(간격 조절 가능), 생성위치 조절, y, z값은 변화 없음
                    }
                    break;
                case 3:
                    bulletObj = objectManager.MakeObj("playerBullet1");
                    bulletObj.transform.position = firePos.position;
                    bulletObj.transform.rotation = Quaternion.identity;
                    for (int i = 0; i < 2; ++i) //집중탄
                    {
                        bulletObj = objectManager.MakeObj("playerBullet0");
                        bulletObj.transform.position = firePos.position + new Vector3(((1.0f - 2) / 2.0f + i) * 0.65f, -0.1f, 0f);
                        bulletObj.transform.rotation = Quaternion.identity;
                        //플레이어가 보는 방향으로 레벨에 비례한 총알 수 발사(간격 조절 가능), 생성위치 조절
                    }
                    break;
                case 4:
                    if (BulletCnt++ == 0)
                    {
                        for (int i = 0; i < 6; ++i) //확산탄
                        {
                            bulletObj = objectManager.MakeObj("playerBullet1");
                            bulletObj.transform.SetPositionAndRotation(firePos.position, Quaternion.Euler(0f, 0f, ((1.0f - 6) / 2.0f + i) * 5.0f));
                            //총알 생성시 각도 조절, 각도는 z방향 회전, Quaternion 오일러 값
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 6; ++i) //확산탄
                        {
                            bulletObj = objectManager.MakeObj("playerBullet0");
                            bulletObj.transform.SetPositionAndRotation(firePos.position, Quaternion.Euler(0f, 0f, ((1.0f - 6) / 2.0f + i) * 5.0f));
                            //총알 생성시 각도 조절, 각도는 z방향 회전, Quaternion 오일러 값
                        }
                    }
                    BulletCnt = BulletCnt > 5 ? 0 : BulletCnt;
                    break;
            }
        }
        if(powerIter > 2f)
        {
            AddFollower();
            power = 3;
            powerIter = 0;
        }
        iter = iter > 0.1 ? 0 : iter; //발사 후 딜레이 초기화
    }
    void AddFollower()
    {
        for (int i = 0; i < followers.Length; i++)
        {
            if (!followers[i].activeSelf)
            {
                followers[i].SetActive(true);
                return;
            }
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
                    boomEffect.SetActive(true); //파괴 이펙트 실행
                    Invoke("BoomEffectOff", 2f);

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

                    //적 총알
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
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Enemy Bullet" || collision.tag == "Boss Bullet") //적 총알에 맞았을 때 실행
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
            gameObject.SetActive(false); //비활성화
            if (destructionEffectPrefab != null) // 파괴 이펙트 생성
            {
                GameObject destructionEffect = objectManager.MakeObj("destroyEffect");
                destructionEffect.transform.position = transform.position;
            }
            gameManager.RespawnPlayer(); //2초 후 부활
        }
        else if(collision.tag == "PowerItem") //아이템을 먹었을 때
        {
            ++power;
            power = power > 4 ? 4 : power;
            if (power > 3)
                powerIter = 0;
            gameManager.score += 30;
        }
        else if(collision.tag == "BoomItem")
        {
            boom = boom < 3 ? ++boom : 3;
            gameManager.UpdateBoomIcon(boom);
            gameManager.score += 50;
        }
        else if(collision.tag == "Coin")
        {
            gameManager.score += 500;
        }
    }
    void BoomEffectOff()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }
}