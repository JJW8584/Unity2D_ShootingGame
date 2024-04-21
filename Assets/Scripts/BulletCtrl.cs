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
            transform.Translate(Vector2.up * b_speed * Time.deltaTime, Space.Self); //총알이 바라보는 방향으로 발사
        }
        else
        {
            transform.Translate(Vector2.down * b_speed * Time.deltaTime, Space.Self); //총알이 바라보는 방향으로 발사
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet Border") //화면 밖 보더에 닿을 시 파괴
            gameObject.SetActive(false);
        else if (gameObject.tag == "Bullet" && collision.gameObject.tag == "Enemy") //적과 충돌 시 파괴
            gameObject.SetActive(false);
        else if ((gameObject.tag == "Enemy Bullet" || gameObject.tag == "Boss Bullet") && collision.gameObject.tag == "Player") //플레이어가 맞을 시 파괴
            gameObject.SetActive(false);
    }
}
