using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) //Collision���� �پ��� ������ �������(�浹����, �ӵ� ��)
    {
        if(collision.collider.tag == "Bullet") 
        {
            Debug.Log("Attacked1");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Collider���� �浹 ���θ� üũ
    {
        if (collision.tag == "Bullet") //�Ҹ��� �浹�ϸ� �α� �޽��� ���
        {
            Debug.Log("Attacked2");
        }
    }
}
