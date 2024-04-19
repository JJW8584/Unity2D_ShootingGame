using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyAPrefab;
    public GameObject enemyBPrefab;
    public GameObject enemyCPrefab;
    public GameObject bossPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemBoomPrefab;
    public GameObject itemPowerPrefab;
    public GameObject playerBullet0Prefab;
    public GameObject playerBullet1Prefab;
    public GameObject followerBulletPrefab;
    public GameObject enemyBullet0Prefab;
    public GameObject enemyBullet1Prefab;
    public GameObject bossBullet0Prefab;
    public GameObject bossBullet1Prefab;
    public GameObject destroyEffectPrefab;
    public Transform[] itemWayPoints;

    GameObject[] enemyA;
    GameObject[] enemyB;
    GameObject[] enemyC;
    GameObject[] boss;

    GameObject[] itemCoin;
    GameObject[] itemBoom;
    GameObject[] itemPower;

    GameObject[] playerBullet0;
    GameObject[] playerBullet1;
    GameObject[] followerBullet;
    GameObject[] enemyBullet0;
    GameObject[] enemyBullet1;
    GameObject[] bossBullet0;
    GameObject[] bossBullet1;

    GameObject[] destroyEffect;

    GameObject[] targetPool;

    private void Awake()
    {
        enemyA = new GameObject[20];
        enemyB = new GameObject[10];
        enemyC = new GameObject[10];
        boss = new GameObject[1];

        itemBoom = new GameObject[10];
        itemCoin = new GameObject[10];
        itemPower = new GameObject[10];

        playerBullet0 = new GameObject[100];
        playerBullet1 = new GameObject[100];
        followerBullet = new GameObject[50];
        enemyBullet0 = new GameObject[100];
        enemyBullet1 = new GameObject[100];
        bossBullet0 = new GameObject[50];
        bossBullet1 = new GameObject[200];

        destroyEffect = new GameObject[40];

        Generate();
    }

    void Generate()
    {
        //Enemy
        for (int i = 0; i < enemyA.Length; i++)
        {
            enemyA[i] = Instantiate(enemyAPrefab);
            enemyA[i].SetActive(false);
        }
        for (int i = 0; i < enemyB.Length; i++)
        {
            enemyB[i] = Instantiate(enemyBPrefab);
            enemyB[i].SetActive(false);
        }
        for (int i = 0; i < enemyC.Length; i++)
        {
            enemyC[i] = Instantiate(enemyCPrefab);
            enemyC[i].SetActive(false);
        }
        for (int i = 0; i < boss.Length; i++)
        {
            boss[i] = Instantiate(bossPrefab);
            boss[i].SetActive(false);
        }

        //Item
        for (int i = 0; i < itemBoom.Length; i++)
        {
            itemBoom[i] = Instantiate(itemBoomPrefab);
            ItemCtrl itemLogic = itemBoom[i].GetComponent<ItemCtrl>();
            itemLogic.wayPoints = itemWayPoints;
            itemBoom[i].SetActive(false);
        }
        for (int i = 0; i < itemCoin.Length; i++)
        {
            itemCoin[i] = Instantiate(itemCoinPrefab);
            ItemCtrl itemLogic = itemCoin[i].GetComponent<ItemCtrl>();
            itemLogic.wayPoints = itemWayPoints;
            itemCoin[i].SetActive(false);
        }
        for (int i = 0; i < itemPower.Length; i++)
        {
            itemPower[i] = Instantiate(itemPowerPrefab);
            ItemCtrl itemLogic = itemPower[i].GetComponent<ItemCtrl>();
            itemLogic.wayPoints = itemWayPoints;
            itemPower[i].SetActive(false);
        }

        //Bullet
        for (int i = 0; i < playerBullet0.Length; i++)
        {
            playerBullet0[i] = Instantiate(playerBullet0Prefab);
            playerBullet0[i].SetActive(false);
        }
        for (int i = 0; i < playerBullet1.Length; i++)
        {
            playerBullet1[i] = Instantiate(playerBullet1Prefab);
            playerBullet1[i].SetActive(false);
        }
        for (int i = 0; i < followerBullet.Length; i++)
        {
            followerBullet[i] = Instantiate(followerBulletPrefab);
            followerBullet[i].SetActive(false);
        }
        for (int i = 0; i < enemyBullet0.Length; i++)
        {
            enemyBullet0[i] = Instantiate(enemyBullet0Prefab);
            enemyBullet0[i].SetActive(false);
        }
        for (int i = 0; i < enemyBullet1.Length; i++)
        {
            enemyBullet1[i] = Instantiate(enemyBullet1Prefab);
            enemyBullet1[i].SetActive(false);
        }
        for (int i = 0; i < bossBullet0.Length; i++)
        {
            bossBullet0[i] = Instantiate(bossBullet0Prefab);
            bossBullet0[i].SetActive(false);
        }
        for (int i = 0; i < bossBullet1.Length; i++)
        {
            bossBullet1[i] = Instantiate(bossBullet1Prefab);
            bossBullet1[i].SetActive(false);
        }

        //Effect
        for (int i = 0; i < destroyEffect.Length; i++)
        {
            destroyEffect[i] = Instantiate(destroyEffectPrefab);
            destroyEffect[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string Type)
    {
        switch (Type)
        {
            case "enemyA":
                targetPool = enemyA;
                break;
            case "enemyB":
                targetPool = enemyB;
                break;
            case "boss":
                targetPool = boss;
                break;
            case "enemyC":
                targetPool = enemyC;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "playerBullet0":
                targetPool = playerBullet0;
                break;
            case "playerBullet1":
                targetPool = playerBullet1;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "enemyBullet0":
                targetPool = enemyBullet0;
                break;
            case "enemyBullet1":
                targetPool = enemyBullet1;
                break;
            case "bossBullet0":
                targetPool = bossBullet0;
                break;
            case "bossBullet1":
                targetPool = bossBullet1;
                break;
            case "destroyEffect":
                targetPool = destroyEffect;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string Type)
    {
        switch (Type)
        {
            case "enemyA":
                targetPool = enemyA;
                break;
            case "enemyB":
                targetPool = enemyB;
                break;
            case "boss":
                targetPool = boss;
                break;
            case "enemyC":
                targetPool = enemyC;
                break;
            case "itemBoom":
                targetPool = itemBoom;
                break;
            case "itemCoin":
                targetPool = itemCoin;
                break;
            case "itemPower":
                targetPool = itemPower;
                break;
            case "playerBullet0":
                targetPool = playerBullet0;
                break;
            case "playerBullet1":
                targetPool = playerBullet1;
                break;
            case "followerBullet":
                targetPool = followerBullet;
                break;
            case "enemyBullet0":
                targetPool = enemyBullet0;
                break;
            case "enemyBullet1":
                targetPool = enemyBullet1;
                break;
            case "bossBullet0":
                targetPool = bossBullet0;
                break;
            case "bossBullet1":
                targetPool = bossBullet1;
                break;
            case "destroyEffect":
                targetPool = destroyEffect;
                break;
        }
        return targetPool;
    }
}
