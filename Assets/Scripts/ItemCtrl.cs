using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCtrl : MonoBehaviour
{
    public Transform[] wayPoints; // ������ ���
    public Transform[] cloneWayPoints; // �̵���θ� ������ �迭

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
        if(!isWayPointSet)
        {
            CalBazierStart();
            isWayPointSet = true;
        }
        if (t < 1f)
        {
            transform.position = Mathf.Pow(1 - t, 3) * cloneWayPoints[0].position  //������ ��� ���� �̵�
                    + 3 * t * Mathf.Pow(1 - t, 2) * cloneWayPoints[1].position
                    + 3 * Mathf.Pow(t, 2) * (1 - t) * cloneWayPoints[2].position
                    + Mathf.Pow(t, 3) * cloneWayPoints[3].position;
        }
        else
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
    private void OnTriggerEnter2D(Collider2D collision)
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
            if (cloneWayPoints[i] == null)
            {
                cloneWayPoints[i] = new GameObject("Waypoint_" + i).transform;
            }
            cloneWayPoints[i].position = wayPoints[i].position + transform.position; // ������ ������ġ ����
        }
    }
}
