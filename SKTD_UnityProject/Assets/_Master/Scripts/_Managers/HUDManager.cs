using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;
using System;

public class HUDManager : MonoBehaviour
{
    PooledWeapon _pooledWeapon;
    PlayerController _playerController;
    EconomyManager _economyManager;
    StoreManager _storeManager;

    [Header("Store Buttons")]
    [SerializeField] StoreButtonsStruct[] _storeButtons;


    [Header("Health Values")]
    [SerializeField] Image _shieldFill;
    [SerializeField] Image _healthFill;

    [Header("Ammo Values")]
    [SerializeField] private TextMeshProUGUI _missileCounter;

    [Header("Coin Values")]
    [SerializeField] private TextMeshProUGUI _coinCounter;

    [Header("Store Values")]
    [SerializeField] GameObject _storePanel;
    [SerializeField] private string _storePanelInAnim = "PanelStoreIn";
    [SerializeField] private string _storePanelOutAnim = "PanelStoreOut";

    [Header("Game Over Values")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private string _gameOverPanelAnim = "PanelGameOverIn";
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _menuButton;

    private void Awake()
    {
        _storeManager = FindAnyObjectByType<StoreManager>();
        _pooledWeapon = FindAnyObjectByType<PlayerController>().GetComponentInChildren<PooledWeapon>();
        _playerController = FindAnyObjectByType<PlayerController>();
        _economyManager = FindAnyObjectByType<EconomyManager>();
    }

    private void OnEnable()
    {
        _pooledWeapon.OnAmmoChanging += SetMissileValue;
        _playerController.OnHealthUpdated += SetHealthValues;
        _playerController.OnDeath += GameOverIn;
        _economyManager.OnMoneyChanged += UpdateMoneyCounter;
        
    }

    private void OnDisable()
    {
        _pooledWeapon.OnAmmoChanging -= SetMissileValue;
        _playerController.OnHealthUpdated -= SetHealthValues;
        _playerController.OnDeath -= GameOverIn;
        _economyManager.OnMoneyChanged -= UpdateMoneyCounter;


    }

    private void Start()
    {
        SetMissileValue();
        SetHealthValues();
        UpdateMoneyCounter(_economyManager.currentMoney);
        SetStoreButtons();
        SetGameOverButtons();
    }

    public void SetHealthValues()
    {
        _shieldFill.fillAmount = (float)_playerController.currentShield / _playerController.maxShield;
        _healthFill.fillAmount = (float)_playerController.currentHealth / _playerController.maxHealth;
    }

    public void SetMissileValue()
    {
        string currentAmmo = _pooledWeapon.currentAmmo.ToString();
        string maxAmmo = _pooledWeapon.maxAmmo.ToString();
        _missileCounter.text = currentAmmo+"/"+maxAmmo;
    }

 

    private void UpdateMoneyCounter(int newAmount)
    {
        _coinCounter.text = "x"  + newAmount.ToString();
    }

    [ContextMenu("Store In")]
    public void StoreIn()
    {
        _storePanel.GetComponent<Animator>().Play(_storePanelInAnim);
    }
    [ContextMenu("Store Out")]
    public void StoreOut()
    {
        _storePanel.GetComponent<Animator>().Play(_storePanelOutAnim);
    }

    public void SetGameOverButtons()
    {
        _retryButton.onClick.AddListener(() =>{ SceneFlowManager.Instance.LoadLevel("SampleScene"); });
        _menuButton.onClick.AddListener(() =>{ SceneFlowManager.Instance.LoadLevel("MainMenu"); });
    }
    public void GameOverIn()
    {
        _gameOverPanel.GetComponent<Animator>().Play(_gameOverPanelAnim);

    }

    public void SetStoreButtons()
    {
        foreach (var button in _storeButtons)
        {
            for (int i = 0; i < _storeManager.StoreEvents.Length; i++)
            {
                if (button.purchaseID == _storeManager.StoreEvents[i].ID)
                {
                    int currentIndex = i; 
                    button.currentButton.onClick.AddListener(() =>
                    {
                        _storeManager.InvokePurchaseEvent(_storeManager.StoreEvents[currentIndex], button.buttonName);
                    });
                }
            }
        }

    }
}
    [Serializable]
    public struct StoreButtonsStruct
    {
        public int purchaseID;
        public string buttonName;
        public Button currentButton;
    } 
