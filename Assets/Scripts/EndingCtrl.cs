using UnityEngine;
using TMPro;

public class EndingCtrl : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private float destroyIter = 0;
    private int destroyCnt = 0;
    private bool ending = false;

    public GameObject destroyEffectPrefab;
    GameObject[] destroyEffect;

    public GameObject endingSet;

    int score;
    float playerX;
    float playerY;

    private void Load() //Play 씬의 정보를 가져옴
    {
        score = PlayerPrefs.GetInt("score");
        playerX = PlayerPrefs.GetFloat("playerX");
        playerY = PlayerPrefs.GetFloat("playerY");
    }
    private void Awake()
    {
        Load(); //Play 씬의 정보 로드
        scoreText.text = string.Format("{0:n0}", score); //점수 출력
        transform.position = new Vector3(playerX, playerY); //player의 위치 받아옴
        destroyEffect = new GameObject[40]; //파괴 이펙트 풀링
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        destroyIter += Time.deltaTime;
        if (destroyIter > 0.1f) //보스가 파괴되는 연출
        {
            EndingPerfomance();
            destroyIter = 0;
            destroyCnt++;
        }
        if (destroyCnt > 20 && !ending) //보스가 파괴되면 날아감
        {
            PlayerGo();
        }
    }
    void EndingPerfomance() //보스 파괴 이펙트
    {
        for(int i = 0; i < 2; i++)
        {
            Vector3 destroyPos = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(1.5f, 4f), 0); //랜덤한 위치 지정
            GameObject destructionEffect = MakeDestroyEffect(); //랜덤한 위치에 파괴 이펙트
            destructionEffect.transform.position = destroyPos;
        }
    }
    void PlayerGo() //플레이어가 날아가는 연출
    {
        transform.Translate(Vector2.up * 10 * Time.deltaTime);
        if (transform.position.y > 7f)
        {
            gameObject.SetActive(false);
            ending = true;
            endingSet.SetActive(true);
        }
    }
    void Generate() //풀링
    {
        //Effect
        for (int i = 0; i < destroyEffect.Length; i++)
        {
            destroyEffect[i] = Instantiate(destroyEffectPrefab);
            destroyEffect[i].SetActive(false);
        }
    }
    public GameObject MakeDestroyEffect() //파괴 이펙트 생성
    {
        for (int i = 0; i < destroyEffect.Length; i++)
        {
            if (!destroyEffect[i].activeSelf)
            {
                destroyEffect[i].SetActive(true);
                return destroyEffect[i];
            }
        }
        return null;
    }
}
