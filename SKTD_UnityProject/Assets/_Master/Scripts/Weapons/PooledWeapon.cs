using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledWeapon : MonoBehaviour
{
    PoolManager _poolManager;

    [SerializeField] private float _projectileForce = 10f;

    public int maxAmmo = 3;
    private int _currentAmmo;


    private void Awake()
    {
        _poolManager = FindAnyObjectByType<PoolManager>();
    }

    private void Start()
    {
        _currentAmmo = maxAmmo; 
    }

    public void ShootProjectile(Transform spawnPos)
    {
       GameObject projectile =  _poolManager.GetMissile();
       projectile.transform.position = spawnPos.position;
       projectile.transform.rotation = spawnPos.rotation;
        projectile.SetActive(true); 
    }


    
}
