using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCtrl : MonoBehaviour
{
    void AnimationEndEvent() //애니메이션이 1번 재생 후 실행
    {
        gameObject.SetActive(false);
    }
}
