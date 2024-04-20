using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public bl_Joystick js; // ���̽�ƽ ������Ʈ�� ������ ������� �����ϱ�.
    public float speed; // ���̽�ƽ�� ���� ������ ������Ʈ�� �ӵ�.

    void Update()
    {
        // ��ƽ�� �����ִ� ������ ����
        Vector3 dir = new Vector3(js.Horizontal, js.Vertical, 0);

        // Vector�� ������ ���������� ũ�⸦ 1�� ���δ�. ���̰� ����ȭ ���� ������ 0���� ����.
        dir.Normalize();

        // ������Ʈ�� ��ġ�� dir �������� �̵���Ų��.
        transform.position += dir * speed * Time.deltaTime;
    }
}
