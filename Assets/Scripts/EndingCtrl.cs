using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System.Runtime.Serialization;

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

    private void Load()
    {
        score = PlayerPrefs.GetInt("score");
        playerX = PlayerPrefs.GetFloat("playerX");
        playerY = PlayerPrefs.GetFloat("playerY");
    }
    private void Awake()
    {
        Load();
        scoreText.text = string.Format("{0:n0}", score);
        transform.position = new Vector3(playerX, playerY);
        destroyEffect = new GameObject[40];
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        destroyIter += Time.deltaTime;
        if (destroyIter > 0.1f)
        {
            EndingPerfomance();
            destroyIter = 0;
            destroyCnt++;
        }
        if (destroyCnt > 20 && !ending)
        {
            PlayerGo();
        }
    }
    void EndingPerfomance()
    {
        for(int i = 0; i < 2; i++)
        {
            Vector3 destroyPos = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(1.5f, 4f), 0);
            GameObject destructionEffect =MakeDestroyEffect();
            destructionEffect.transform.position = destroyPos;
        }
    }
    void PlayerGo()
    {
        transform.Translate(Vector2.up * 10 * Time.deltaTime);
        if (transform.position.y > 7f)
        {
            gameObject.SetActive(false);
            ending = true;
            endingSet.SetActive(true);
        }
    }
    void Generate()
    {
        //Effect
        for (int i = 0; i < destroyEffect.Length; i++)
        {
            destroyEffect[i] = Instantiate(destroyEffectPrefab);
            destroyEffect[i].SetActive(false);
        }
    }
    public GameObject MakeDestroyEffect()
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
