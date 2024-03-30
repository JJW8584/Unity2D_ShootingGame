using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour
{
    [SerializeField] private int bg_speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * bg_speed * Time.deltaTime, Space.Self); //�Ʒ��������� ������
        if(transform.position.y < -12.0f)
        {
            transform.position = transform.position + new Vector3(0f, 24.0f, 0f); //����� 2���̹Ƿ� 24��ŭ �̵�
        }
    }
}
