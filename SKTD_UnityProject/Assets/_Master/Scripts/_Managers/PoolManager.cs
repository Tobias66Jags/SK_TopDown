using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject _missile;
    [SerializeField] private GameObject _enemyProjectile;

    [SerializeField] private int _missilesAmount = 3;
    [SerializeField] private int _enemyProjectileAmount = 30;


    private List<GameObject> _missiles = new List<GameObject>();
    private List<GameObject> _eProjectiles = new List<GameObject>();

    private void Start()
    {
       InstantiateProjectiles(_missiles, _missile, _missilesAmount);
       InstantiateProjectiles(_eProjectiles, _enemyProjectile, _enemyProjectileAmount);
    }


    public void InstantiateProjectiles(List<GameObject> currentList, GameObject projectile, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newProjectile = Instantiate(projectile);
            currentList.Add(newProjectile); 
            newProjectile.SetActive(false);
        }
    }

    public GameObject GetMissile()
    {
        return GetPooledProjectile(_missiles);
    }
    public GameObject GetEnemyProjectile() 
    {
        return GetPooledProjectile(_eProjectiles);   
    }

    public GameObject GetPooledProjectile (List<GameObject> list)
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
