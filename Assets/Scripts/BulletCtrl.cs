using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private int b_speed = 10;
    public int dmg = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * b_speed * Time.deltaTime, Space.Self); //�Ѿ��� �ٶ󺸴� �������� �߻�
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet Border") //ȭ�� �� ������ ���� �� �ı�
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Enemy") //���� �浹 �� �ı�
            Destroy(gameObject);
    }
}
