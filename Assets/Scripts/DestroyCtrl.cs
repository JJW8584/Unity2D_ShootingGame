using UnityEngine;

public class DestroyCtrl : MonoBehaviour
{
    void AnimationEndEvent() //�ִϸ��̼��� 1�� ��� �� ����
    {
        gameObject.SetActive(false);
    }
}
