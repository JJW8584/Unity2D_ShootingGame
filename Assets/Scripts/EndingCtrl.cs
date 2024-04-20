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

    private void Load() //Play ���� ������ ������
    {
        score = PlayerPrefs.GetInt("score");
        playerX = PlayerPrefs.GetFloat("playerX");
        playerY = PlayerPrefs.GetFloat("playerY");
    }
    private void Awake()
    {
        Load(); //Play ���� ���� �ε�
        scoreText.text = string.Format("{0:n0}", score); //���� ���
        transform.position = new Vector3(playerX, playerY); //player�� ��ġ �޾ƿ�
        destroyEffect = new GameObject[40]; //�ı� ����Ʈ Ǯ��
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        destroyIter += Time.deltaTime;
        if (destroyIter > 0.1f) //������ �ı��Ǵ� ����
        {
            EndingPerfomance();
            destroyIter = 0;
            destroyCnt++;
        }
        if (destroyCnt > 20 && !ending) //������ �ı��Ǹ� ���ư�
        {
            PlayerGo();
        }
    }
    void EndingPerfomance() //���� �ı� ����Ʈ
    {
        for(int i = 0; i < 2; i++)
        {
            Vector3 destroyPos = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(1.5f, 4f), 0); //������ ��ġ ����
            GameObject destructionEffect = MakeDestroyEffect(); //������ ��ġ�� �ı� ����Ʈ
            destructionEffect.transform.position = destroyPos;
        }
    }
    void PlayerGo() //�÷��̾ ���ư��� ����
    {
        transform.Translate(Vector2.up * 10 * Time.deltaTime);
        if (transform.position.y > 7f)
        {
            gameObject.SetActive(false);
            ending = true;
            endingSet.SetActive(true);
        }
    }
    void Generate() //Ǯ��
    {
        //Effect
        for (int i = 0; i < destroyEffect.Length; i++)
        {
            destroyEffect[i] = Instantiate(destroyEffectPrefab);
            destroyEffect[i].SetActive(false);
        }
    }
    public GameObject MakeDestroyEffect() //�ı� ����Ʈ ����
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
