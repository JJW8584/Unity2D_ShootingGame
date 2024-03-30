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

    private void OnCollisionEnter2D(Collision2D collision) //Collision값은 다양한 정보가 들어있음(충돌방향, 속도 등)
    {
        if(collision.collider.tag == "Bullet") 
        {
            Debug.Log("Attacked1");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Collider값은 충돌 여부만 체크
    {
        if (collision.tag == "Bullet") //불릿과 충돌하면 로그 메시지 출력
        {
            Debug.Log("Attacked2");
        }
    }
}
