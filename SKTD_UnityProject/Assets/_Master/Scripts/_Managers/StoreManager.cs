using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    EconomyManager _economyManager;
    PlayerController _playerController;
    PooledWeapon _pooledWeapon;

    public StoreEventsStruct[] StoreEvents;

    public delegate void Purchase(int cost);
    public event Purchase OnPurchased;

    private void Awake()
    {
        _playerController = FindAnyObjectByType<PlayerController>();
        _economyManager = FindAnyObjectByType<EconomyManager>();
        _pooledWeapon = _playerController.gameObject.GetComponentInChildren<PooledWeapon>();
    }

    public void InvokePurchaseEvent(StoreEventsStruct currentStoreStruct, string currentEvent)
    {
        if(currentStoreStruct.cost <= _economyManager.currentMoney)
        {
            Invoke(currentEvent,0);
            OnPurchased?.Invoke(currentStoreStruct.cost);
        }
       
    }

    private void RegenerateHealth()
    {
        _playerController.currentHealth = _playerController.maxHealth;
        _playerController.CallHealthUpdate();
    }
    private void IncreaseMaxShield()
    {
        _playerController.maxShield += 50;
        _playerController.CallHealthUpdate();
    }
    private void IncreaseMissile()
    {
        _pooledWeapon.maxAmmo += 1;
        _pooledWeapon.CallAmmoEvent();
    }
    private void IncreaseMaxHealth()
    {
        _playerController.maxHealth += 50;
        _playerController.CallHealthUpdate();
    }
    private void RecoverMissiles()
    {
        _pooledWeapon.currentAmmo = _pooledWeapon.maxAmmo;
        _pooledWeapon.CallAmmoEvent();
    }
    public void ReduceShieldTime()
    {
        _playerController.timeToRegenerateShield -= _playerController.timeToRegenerateShield * 0.1f;
        _playerController.timeToRegenerateShield = Mathf.Max(_playerController.timeToRegenerateShield, 0.5f);
    }
}

[Serializable] 
public struct StoreEventsStruct
{
    public int ID;
    //public string EventName;
    public int cost;
}