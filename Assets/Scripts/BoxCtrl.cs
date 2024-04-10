using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCtrl : MonoBehaviour
{
    private Animator _animator;
    private bool isFirstClick = true;
    private bool isRed = true;

    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRed) //Exam02 3번은 프로젝트 세팅의 그래픽스에서 설정가능
        {
            // 붉은색 Box일 때만 이동
            float h = Input.GetAxis("Horizontal"); //x값
            float v = Input.GetAxis("Vertical"); //y값

            Vector3 moveDir = Vector3.up * v + Vector3.right * h; //방향벡터 객체 선언

            transform.Translate(moveDir * speed * Time.deltaTime); //이동

            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); //월드 좌표를 뷰포트 좌표계로 변환
            viewPos.x = Mathf.Clamp01(viewPos.x); //x값을 0이상, 1이하로 제한
            viewPos.y = Mathf.Clamp01(viewPos.y); //y값을 0이상, 1이하로 제한
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos); //다시 월드 좌표로 변환
            transform.position = worldPos; //좌표를 적용
        }
    }

    private void OnMouseDown() //Exam01
    {
        if (isFirstClick) //첫 번째 or 빨간색일 때 클릭
        {
            isFirstClick = false;
            GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f); // 녹색으로 색 변경
            isRed = false;
            _animator.SetBool("Spin", true); //회전
            _animator.SetBool("Scale", false); //크기변화 멈춤
        }
        else //두 번째 or 녹색일 때 클릭
        {
            isFirstClick = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f); // 빨간색으로 색 변경
            isRed = true;
            _animator.SetBool("Spin", false); //회전 멈춤
            _animator.SetBool("Scale", true); //크기변화
        }
    }
}
