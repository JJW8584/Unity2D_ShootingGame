using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public int speed = 10;

    public GameObject p_Bullet;
    public Transform firePos;

    private int iter = 0;

    public int level = 1;
    public int type = 0;

    private Animator _animator;

    //public Transform myTr;

    // Start is called before the first frame update
    void Start()
    {
        /*Debug.Log("Log");
        Debug.LogWarning("Warning");
        Debug.LogError("Error");*/
        //myTr = GetComponent<Transform>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //x값
        float v = Input.GetAxis("Vertical"); //y값

        _animator.SetFloat("p_Hori", h); //플레이어의 p_Hori값을 set, 애니메이션을 위함

        Vector3 moveDir = Vector3.up * v + Vector3.right * h; //방향벡터 객체 선언

        transform.Translate(moveDir * speed * Time.deltaTime); //플레이어의 이동

        //Debug.Log("Player X = " + transform.position.x.ToString());

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); //캐릭터의 월드 좌표를 뷰포트 좌표계로 변환해준다.
        viewPos.x = Mathf.Clamp01(viewPos.x); //x값을 0이상, 1이하로 제한한다.
        viewPos.y = Mathf.Clamp01(viewPos.y); //y값을 0이상, 1이하로 제한한다.
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos); //다시 월드 좌표로 변환한다.
        transform.position = worldPos; //좌표를 적용한다.

        if (Input.GetMouseButton(0))
        {
            ++iter;
            if (iter % 60 == 0) //60틱에 1번씩 발사
            {
                for (int i = 0; i < level; ++i)
                {
                    if (type == 0) //집중탄
                    {
                        Instantiate(p_Bullet, firePos.position + new Vector3(((1.0f - level) / 2.0f + i) * 0.3f, 0f, 0f), Quaternion.identity);
                        //플레이어가 보는 방향으로 레벨에 비례한 총알 수 발사(간격 조절 가능), 생성위치 조절, y, z값은 변화 없음
                    }
                    else if(type == 1) //확산탄
                    {
                        Instantiate(p_Bullet, firePos.position, Quaternion.Euler(0f, 0f, ((1.0f - level) / 2.0f + i) * 5.0f));
                        //총알 생성시 각도 조절, 각도는 z방향 회전, Quaternion 오일러 값
                    }
                }
            }
        }
    }
}