using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCtrl : MonoBehaviour
{
    public Transform[] wayPoints; // 베지어 경로
    public Transform[] cloneWayPoints; // 이동경로를 복사할 배열

    public PlayerCtrl player;
    public string itemType;

    private float t = 0f;
    private bool isWayPointSet;

    // Start is called before the first frame update
    void Start()
    {
        cloneWayPoints = new Transform[4];
    }

    // Update is called once per frame
    void Update()
    {
        if(!isWayPointSet) //이동경로 계산이 되지 않았을 때
        {
            CalBazierStart();
            isWayPointSet = true;
        }
        if (t < 1f)
        {
            transform.position = Mathf.Pow(1 - t, 3) * cloneWayPoints[0].position  //베지어 곡선을 따라 이동
                    + 3 * t * Mathf.Pow(1 - t, 2) * cloneWayPoints[1].position
                    + 3 * Mathf.Pow(t, 2) * (1 - t) * cloneWayPoints[2].position
                    + Mathf.Pow(t, 3) * cloneWayPoints[3].position;
        }
        else //이동이 끝났을 때 = 플레이어가 아이템을 먹지 않았을 때
        {
            t = 0;
            gameObject.SetActive(false);
        }
        t += Time.deltaTime / 6f;
    }

    private void OnEnable()
    {
        isWayPointSet = false;
    }
    private void OnTriggerEnter2D(Collider2D collision) //플레이어가 먹으면 비활성화
    {
        if (collision.tag == "Player")
        {
            t = 0;
            gameObject.SetActive(false);
        }
    }
    
    public void CalBazierStart()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (cloneWayPoints[i] == null) //이동경로 할당이 되어있지 않은 경우
            {
                cloneWayPoints[i] = new GameObject("Waypoint_" + i).transform;
            }
            cloneWayPoints[i].position = wayPoints[i].position + transform.position; // 아이템 생성위치 보정
        }
    }
}
