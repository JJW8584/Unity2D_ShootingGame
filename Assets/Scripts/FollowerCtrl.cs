using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class FollowerCtrl : MonoBehaviour
{
    private float iter = 0;

    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;
    public Vector3 correctionPos;
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
    void Watch()
    {
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position + correctionPos);

        if(parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if(parentPos.Count < followDelay)
            followPos = parent.position + correctionPos;
    }
    void Follow()
    {
        transform.position = followPos;
    }
    
    void Fire()
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
