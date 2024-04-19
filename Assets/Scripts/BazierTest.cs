using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazierTest : MonoBehaviour
{
    public Transform[] wayPoints; //각 점
    private Vector2 gizmosPosition;

   /* private void OnDrawGizmos()
    {
        for (int i = 0; i < 13; i += 4)
        {
            for (float t = 0; t < 1; t += 0.02f) //베지에 곡선 그리기
            {
                gizmosPosition = Mathf.Pow(1 - t, 3) * wayPoints[i].position
                    + 3 * t * Mathf.Pow(1 - t, 2) * wayPoints[i + 1].position
                    + 3 * Mathf.Pow(t, 2) * (1 - t) * wayPoints[i + 2].position
                    + Mathf.Pow(t, 3) * wayPoints[i + 3].position;

                Gizmos.DrawSphere(gizmosPosition, 0.1f);
            }
            Gizmos.DrawLine(new Vector2(wayPoints[i].position.x, wayPoints[i].position.y), //각 포인트 연결하는 직선
                            new Vector2(wayPoints[i + 1].position.x, wayPoints[i + 1].position.y));
            Gizmos.DrawLine(new Vector2(wayPoints[i + 1].position.x, wayPoints[i + 1].position.y),
                            new Vector2(wayPoints[i + 2].position.x, wayPoints[i + 2].position.y));
            Gizmos.DrawLine(new Vector2(wayPoints[i + 2].position.x, wayPoints[i + 2].position.y),
                            new Vector2(wayPoints[i + 3].position.x, wayPoints[i + 3].position.y));
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
