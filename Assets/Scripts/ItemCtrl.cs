using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCtrl : MonoBehaviour
{
    public Transform[] wayPoints; //������ ����Ʈ �̵����
    public Transform[] cloneWayPoints; // �̵���θ� ������ �迭

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
            transform.position = Mathf.Pow(1 - t, 3) * cloneWayPoints[0].position  //������ ��� ���� �̵�
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
        cloneWayPoints = new Transform[wayPoints.Length]; // ���ο� �迭�� �������� ���� ��� �������� �߸��� ��ǥ�� ����
        for (int i = 0; i < wayPoints.Length; i++)
        {
            // ���ο� ��� �����Ͽ� ����
            cloneWayPoints[i] = Instantiate(wayPoints[i], wayPoints[i].position, Quaternion.identity);
            cloneWayPoints[i].position += transform.position; // ������ ������ġ ����
        }
    }
}
