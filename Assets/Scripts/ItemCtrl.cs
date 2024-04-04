using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCtrl : MonoBehaviour
{
    public Transform[] wayPoints; //베지어 포인트 이동경로
    public Transform[] cloneWayPoints; // 이동경로를 복사할 배열

    private float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CalBazierStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (t < 1f)
        {
            transform.position = Mathf.Pow(1 - t, 3) * cloneWayPoints[0].position  //베지어 곡선을 따라 이동
                    + 3 * t * Mathf.Pow(1 - t, 2) * cloneWayPoints[1].position
                    + 3 * Mathf.Pow(t, 2) * (1 - t) * cloneWayPoints[2].position
                    + Mathf.Pow(t, 3) * cloneWayPoints[3].position;
        }
        else
        {
            Destroy(gameObject);
        }

        t += Time.deltaTime / 6f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    void CalBazierStart()
    {
        cloneWayPoints = new Transform[wayPoints.Length]; // 새로운 배열을 생성하지 않을 경우 아이템이 잘못된 좌표에 생성
        for (int i = 0; i < wayPoints.Length; i++)
        {
            // 새로운 요소 생성하여 복사
            cloneWayPoints[i] = Instantiate(wayPoints[i], wayPoints[i].position, Quaternion.identity);
            cloneWayPoints[i].position += transform.position; // 아이템 생성위치 보정
        }
    }
}
