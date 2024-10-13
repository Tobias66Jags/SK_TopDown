using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledWeapon : MonoBehaviour
{
    PoolManager _poolManager;
    private bool _canShoot = true;
    [SerializeField] private float _coolDown = 2;

    public int maxAmmo = 3;
    public int currentAmmo;

    public delegate void AmmoEventChanging();
    public event AmmoEventChanging OnAmmoChanging;

    private void Awake()
    {
        _poolManager = FindAnyObjectByType<PoolManager>();
        currentAmmo = maxAmmo;
    }

    private void Start()
    {
        CallAmmoEvent();
    }

    public void ShootMissile(Transform spawnPos)
    {
        if (_canShoot && currentAmmo != 0)
        {
            _canShoot = false;
            currentAmmo--;
            GameObject projectile = _poolManager.GetMissile();
            projectile.transform.position = spawnPos.position;
            projectile.transform.rotation = spawnPos.rotation;
            projectile.SetActive(true);
            AudioManager.Instance.PlaySound("Missile");
            CallAmmoEvent();
            Invoke("Cooling", _coolDown);
        }

    }

    public void Cooling()
    {
        _canShoot = true;
    }

    public void CallAmmoEvent()
    {
        if (OnAmmoChanging != null) OnAmmoChanging();
    }
}
