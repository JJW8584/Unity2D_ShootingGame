using UnityEngine;

public class BgScroll : MonoBehaviour
{
    [SerializeField] private int bg_speed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * bg_speed * Time.deltaTime, Space.Self); //아래방향으로 움직임
        if(transform.position.y < -12.0f)
        {
            transform.position += new Vector3(0f, 24.0f, 0f); //배경이 2개이므로 24만큼 이동
        }
    }
}
