using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class FollowerCtrl : MonoBehaviour
{
    private float iter = 0;

    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay; //���󰡴� ������(�ð���)
    public Transform parent; //�÷��̾��� ��ġ
    public Queue<Vector3> parentPos; //�����̸� ���� ť
    public Vector3 correctionPos; //�÷��̾� ���� ������Ʈ�� ����Ʈ ��ġ
    private void Awake()
    {
        parentPos = new Queue<Vector3>();
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        Watch();
        Follow();
        Fire();
    }
    void Watch() //�÷��̾��� ��ġ�� ã��
    {
        if(!parentPos.Contains(parent.position)) //���� ���°� �ƴ� ��
            parentPos.Enqueue(parent.position + correctionPos); //ť�� �Է�

        //ť�� ����Ͽ� ��������� �����̸� �ΰ� �÷��̾ ����
        if (parentPos.Count > followDelay) //ť�� �� á����(������) ��ǥ �̵�
            followPos = parentPos.Dequeue();
        else if(parentPos.Count < followDelay) //�� á�� ��(���� ��ü�� ��� �������� ��) �÷��̾ ����
            followPos = parent.position + correctionPos;
    }
    void Follow() //�÷��̾ ����
    {
        transform.position = followPos;
    }
    
    void Fire() //1�ʸ��� �Ѿ� �߻�
    {
        iter += Time.deltaTime;
        if (iter > 1f)
        {
            GameObject Bullet = objectManager.MakeObj("followerBullet");
            Bullet.transform.position = transform.position;
            iter = 0;
        }
    }
}
