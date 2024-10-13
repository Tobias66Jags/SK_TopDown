using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    StoreManager _storeManager;

    [SerializeField] private int initialMoney = 100;
    public int currentMoney;

 

    public delegate void MoneyChanged(int newAmount);
    public event MoneyChanged OnMoneyChanged;

    private void Awake()
    {
        currentMoney = initialMoney;
        _storeManager = FindAnyObjectByType<StoreManager>();
    }
    private void OnEnable()
    {
        _storeManager.OnPurchased += MakePurchase;
    }
    private void OnDisable()
    {
        _storeManager.OnPurchased -= MakePurchase;
    }

    private void Start()
    {
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void MakePurchase(int cost)
    {
        currentMoney -= cost;
        OnMoneyChanged?.Invoke(currentMoney);
    }
    public void MakeRefund(int cost)
    {
        currentMoney += cost;
        OnMoneyChanged?.Invoke(currentMoney);
    }

    public void GetEnemyAmount(int cost)
    {
        currentMoney += cost;
        OnMoneyChanged?.Invoke(currentMoney);
    }
}
