using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public bl_Joystick js; // 조이스틱 오브젝트를 저장할 변수
    public float speed; // 조이스틱에 의해 움직일 오브젝트의 속도

    void Update()
    {
        // 스틱이 향해있는 방향을 저장
        Vector3 dir = new Vector3(js.Horizontal, js.Vertical, 0);

        // 오브젝트의 위치 이동
        transform.position += dir * speed * Time.deltaTime;
    }
}
