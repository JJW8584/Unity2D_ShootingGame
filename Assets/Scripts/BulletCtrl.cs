using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    [SerializeField] private int b_speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * b_speed * Time.deltaTime, Space.Self);
    }
    private void OnBecameInvisible() //화면 밖으로 나가면 객체 파괴
    {
        Destroy(this.gameObject);
    }
}
