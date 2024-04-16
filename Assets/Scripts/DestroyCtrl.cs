using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AnimationEndEvent() //애니메이션이 1번 재생 후 실행
    {
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
