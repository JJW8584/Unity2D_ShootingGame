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
        float h = Input.GetAxis("Horizontal"); //x��
        float v = Input.GetAxis("Vertical"); //y��

        _animator.SetFloat("p_Hori", h); //�÷��̾��� p_Hori���� set, �ִϸ��̼��� ����

        Vector3 moveDir = Vector3.up * v + Vector3.right * h; //���⺤�� ��ü ����

        transform.Translate(moveDir * speed * Time.deltaTime); //�÷��̾��� �̵�

        //Debug.Log("Player X = " + transform.position.x.ToString());

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position); //ĳ������ ���� ��ǥ�� ����Ʈ ��ǥ��� ��ȯ���ش�.
        viewPos.x = Mathf.Clamp01(viewPos.x); //x���� 0�̻�, 1���Ϸ� �����Ѵ�.
        viewPos.y = Mathf.Clamp01(viewPos.y); //y���� 0�̻�, 1���Ϸ� �����Ѵ�.
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos); //�ٽ� ���� ��ǥ�� ��ȯ�Ѵ�.
        transform.position = worldPos; //��ǥ�� �����Ѵ�.

        if (Input.GetMouseButton(0))
        {
            ++iter;
            if (iter % 60 == 0) //60ƽ�� 1���� �߻�
            {
                for (int i = 0; i < level; ++i)
                {
                    if (type == 0) //����ź
                    {
                        Instantiate(p_Bullet, firePos.position + new Vector3(((1.0f - level) / 2.0f + i) * 0.3f, 0f, 0f), Quaternion.identity);
                        //�÷��̾ ���� �������� ������ ����� �Ѿ� �� �߻�(���� ���� ����), ������ġ ����, y, z���� ��ȭ ����
                    }
                    else if(type == 1) //Ȯ��ź
                    {
                        Instantiate(p_Bullet, firePos.position, Quaternion.Euler(0f, 0f, ((1.0f - level) / 2.0f + i) * 5.0f));
                        //�Ѿ� ������ ���� ����, ������ z���� ȸ��, Quaternion ���Ϸ� ��
                    }
                }
            }
        }
    }
}