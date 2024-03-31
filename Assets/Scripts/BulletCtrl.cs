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
        transform.Translate(Vector2.up * b_speed * Time.deltaTime, Space.Self); //총알이 바라보는 방향으로 발사
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet Border") //화면 밖 보더에 닿을 시 파괴
            Destroy(gameObject);
        else if (collision.gameObject.tag == "Enemy") //적과 충돌 시 파괴
            Destroy(gameObject);
    }
}
