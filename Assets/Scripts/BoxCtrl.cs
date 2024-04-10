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
        if (isRed) //Exam02 3���� ������Ʈ ������ �׷��Ƚ����� ��������
        {
            // ������ Box�� ���� �̵�
            float h = Input.GetAxis("Horizontal"); //x��
            float v = Input.GetAxis("Vertical"); //y��

            Vector3 moveDir = Vector3.up * v + Vector3.right * h; //���⺤�� ��ü ����

            transform.Translate(moveDir * speed * Time.deltaTime); //�̵�

            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); //���� ��ǥ�� ����Ʈ ��ǥ��� ��ȯ
            viewPos.x = Mathf.Clamp01(viewPos.x); //x���� 0�̻�, 1���Ϸ� ����
            viewPos.y = Mathf.Clamp01(viewPos.y); //y���� 0�̻�, 1���Ϸ� ����
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos); //�ٽ� ���� ��ǥ�� ��ȯ
            transform.position = worldPos; //��ǥ�� ����
        }
    }

    private void OnMouseDown() //Exam01
    {
        if (isFirstClick) //ù ��° or �������� �� Ŭ��
        {
            isFirstClick = false;
            GetComponent<SpriteRenderer>().color = new Color(0f, 1f, 0f); // ������� �� ����
            isRed = false;
            _animator.SetBool("Spin", true); //ȸ��
            _animator.SetBool("Scale", false); //ũ�⺯ȭ ����
        }
        else //�� ��° or ����� �� Ŭ��
        {
            isFirstClick = true;
            GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f); // ���������� �� ����
            isRed = true;
            _animator.SetBool("Spin", false); //ȸ�� ����
            _animator.SetBool("Scale", true); //ũ�⺯ȭ
        }
    }
}
