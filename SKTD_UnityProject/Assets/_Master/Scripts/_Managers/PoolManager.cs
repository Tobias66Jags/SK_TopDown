using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject _missile;
    [SerializeField] private GameObject _enemyProjectile;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _coinPrefab;

    [SerializeField] private int _missilesAmount = 3;
    [SerializeField] private int _enemyProjectileAmount = 30;
    [SerializeField] private int _enemiesAmount = 30;
    [SerializeField] private int _coinsAmount = 30;


    private List<GameObject> _missiles = new List<GameObject>();
    private List<GameObject> _eProjectiles = new List<GameObject>();
    private List<GameObject> _enemies = new List<GameObject>();
    private List<GameObject> _coins = new List<GameObject>();

    private void Start()
    {
       InstantiateObjects(_missiles, _missile, _missilesAmount);
       InstantiateObjects(_eProjectiles, _enemyProjectile, _enemyProjectileAmount);
       InstantiateObjects(_enemies, _enemyPrefab, _enemiesAmount);
       InstantiateObjects(_coins, _coinPrefab, _coinsAmount);    
    }


    public void InstantiateObjects(List<GameObject> currentList, GameObject currentObject, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newObject = Instantiate(currentObject);
            currentList.Add(newObject); 
            newObject.SetActive(false);
        }
    }

    public GameObject GetMissile()
    {
        return GetPooledObject(_missiles);
    }
    public GameObject GetEnemyProjectile() 
    {
        return GetPooledObject(_eProjectiles);   
    }
    public GameObject GetEnemy()
    {
        return GetPooledObject(_enemies);
    }
    public GameObject GetCoin()
    {
        return GetPooledObject(_coins);
    }

    public GameObject GetPooledObject (List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeInHierarchy)
            {
                return list[i];
            }

        }
        return null;    
    }
}
