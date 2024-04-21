using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class FollowerCtrl : MonoBehaviour
{
    private float iter = 0;

    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay; //따라가는 딜레이(시간차)
    public Transform parent; //플레이어의 위치
    public Queue<Vector3> parentPos; //딜레이를 위한 큐
    public Vector3 correctionPos; //플레이어 기준 오브젝트의 디폴트 위치
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
    void Watch() //플레이어의 위치를 찾음
    {
        if(!parentPos.Contains(parent.position)) //정지 상태가 아닐 때
            parentPos.Enqueue(parent.position + correctionPos); //큐에 입력

        //큐를 사용하여 어느정도의 딜레이를 두고 플레이어를 따라감
        if (parentPos.Count > followDelay) //큐가 다 찼으면(딜레이) 좌표 이동
            followPos = parentPos.Dequeue();
        else if(parentPos.Count < followDelay) //덜 찼을 때(소형 기체가 방금 생성됐을 때) 플레이어를 따라감
            followPos = parent.position + correctionPos;
    }
    void Follow() //플레이어를 따라감
    {
        transform.position = followPos;
    }
    
    void Fire() //1초마다 총알 발사
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
