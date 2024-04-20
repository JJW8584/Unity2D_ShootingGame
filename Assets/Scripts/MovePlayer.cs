using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public bl_Joystick js; // ���̽�ƽ ������Ʈ�� ������ ����
    public float speed; // ���̽�ƽ�� ���� ������ ������Ʈ�� �ӵ�

    void Update()
    {
        // ��ƽ�� �����ִ� ������ ����
        Vector3 dir = new Vector3(js.Horizontal, js.Vertical, 0);

        // ������Ʈ�� ��ġ �̵�
        transform.position += dir * speed * Time.deltaTime;
    }
}
