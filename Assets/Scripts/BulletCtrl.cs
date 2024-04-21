using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private int b_speed;
    public int dmg;

    // Update is called once per frame
    void Update()
    {
        if (tag != "Boss Bullet")
        {
            transform.Translate(Vector2.up * b_speed * Time.deltaTime, Space.Self); //�Ѿ��� �ٶ󺸴� �������� �߻�
        }
        else
        {
            transform.Translate(Vector2.down * b_speed * Time.deltaTime, Space.Self); //�Ѿ��� �ٶ󺸴� �������� �߻�
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet Border") //ȭ�� �� ������ ���� �� �ı�
            gameObject.SetActive(false);
        else if (gameObject.tag == "Bullet" && collision.gameObject.tag == "Enemy") //���� �浹 �� �ı�
            gameObject.SetActive(false);
        else if ((gameObject.tag == "Enemy Bullet" || gameObject.tag == "Boss Bullet") && collision.gameObject.tag == "Player") //�÷��̾ ���� �� �ı�
            gameObject.SetActive(false);
    }
}
