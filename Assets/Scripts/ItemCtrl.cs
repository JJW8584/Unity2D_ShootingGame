using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    public Transform[] wayPoints; //베지어 포인트 이동경로

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
            transform.position = Mathf.Pow(1 - t, 3) * wayPoints[0].position  //베지어 곡선을 따라 이동
                    + 3 * t * Mathf.Pow(1 - t, 2) * wayPoints[1].position
                    + 3 * Mathf.Pow(t, 2) * (1 - t) * wayPoints[2].position
                    + Mathf.Pow(t, 3) * wayPoints[3].position;
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
        wayPoints[0].position += transform.position;
        wayPoints[1].position += transform.position;
        wayPoints[2].position += transform.position;
        wayPoints[3].position += transform.position;
    }
}
