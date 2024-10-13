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
    EnemySpawnManager _enemySpawnManager;
    ScoreManager _scoreManager;

    [Header("Store Buttons")]
    [SerializeField] StoreButtonsStruct[] _storeButtons;


    [Header("Health Values")]
    [SerializeField] Image _shieldFill;
    [SerializeField] Image _healthFill;

    [Header("Ammo Values")]
    [SerializeField] private TextMeshProUGUI _missileCounter;

    [Header("Coin Values")]
    [SerializeField] private TextMeshProUGUI _coinCounter;

    [Header("Score Values")]
    [SerializeField] private TextMeshProUGUI _scoreCounter;

    [Header("Round Value")]
    [SerializeField] private TextMeshProUGUI _roundCounter;

    [Header("Store Values")]
    [SerializeField] GameObject _storePanel;
    [SerializeField] private string _storePanelInAnim = "PanelStoreIn";
    [SerializeField] private string _storePanelOutAnim = "PanelStoreOut";
    [SerializeField] private Button _closeStoreButton;

    [Header("Enemies Values")]
    [SerializeField] private TextMeshProUGUI _enemiesCounter;

    [Header("Game Over Values")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private string _gameOverPanelAnim = "PanelGameOverIn";
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private TextMeshProUGUI _finalScore;

    private void Awake()
    {
        _scoreManager = FindAnyObjectByType<ScoreManager>();
        _storeManager = FindAnyObjectByType<StoreManager>();
        _pooledWeapon = FindAnyObjectByType<PlayerController>().GetComponentInChildren<PooledWeapon>();
        _playerController = FindAnyObjectByType<PlayerController>();
        _economyManager = FindAnyObjectByType<EconomyManager>();
        _enemySpawnManager = FindAnyObjectByType<EnemySpawnManager>();
    }

    private void OnEnable()
    {
        _pooledWeapon.OnAmmoChanging += SetMissileValue;
        _playerController.OnHealthUpdated += SetHealthValues;
        _playerController.OnDeath += GameOverIn;
        _economyManager.OnMoneyChanged += UpdateMoneyCounter;
        _enemySpawnManager.OnEnemyChanged += UpdateEnemiesCounter;
        _scoreManager.OnScoreChanged += UpdateScoreCounter;
        GameManager.Instance.OnRoundChanged += UpdateRoundCounter;
        GameManager.Instance.OnRoundFinished += StoreIn;

    }

    private void OnDisable()
    {
        _pooledWeapon.OnAmmoChanging -= SetMissileValue;
        _playerController.OnHealthUpdated -= SetHealthValues;
        _playerController.OnDeath -= GameOverIn;
        _economyManager.OnMoneyChanged -= UpdateMoneyCounter;
        _enemySpawnManager.OnEnemyChanged -= UpdateEnemiesCounter;
        _scoreManager.OnScoreChanged -= UpdateScoreCounter;
        GameManager.Instance.OnRoundChanged -= UpdateRoundCounter;
        GameManager.Instance.OnRoundFinished -= StoreIn;
    }

    private void Start()
    {
        UpdateMoneyCounter(_economyManager.currentMoney);
        SetStoreButtons();
        SetGameOverButtons();
        UpdateScoreCounter(_scoreManager.score);
        UpdateRoundCounter();
        UpdateEnemiesCounter(_enemySpawnManager.enemiesRemain);
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

    public void UpdateEnemiesCounter(int newAmount)
    {
        _enemiesCounter.text = "Enemies:"+ newAmount.ToString();
    }

    public void UpdateScoreCounter(int newScore)
    {
        _scoreCounter.text = "Score:" + newScore.ToString();
    }

    public void UpdateRoundCounter()
    {
        _roundCounter.text = "Round:"+GameManager.Instance.roundCounter.ToString();
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
        _retryButton.onClick.AddListener(GameManager.Instance.ResetRounds);
        _menuButton.onClick.AddListener(GameManager.Instance.ResetRounds);
    }
    public void GameOverIn()
    {
        _gameOverPanel.GetComponent<Animator>().Play(_gameOverPanelAnim);
        _finalScore.text = "Score:" + _scoreManager.score.ToString(); 
    }

 

    public void SetStoreButtons()
    {
        _closeStoreButton.onClick.AddListener(StoreOut);
        _closeStoreButton.onClick.AddListener(GameManager.Instance.IncreaseRound);
        _closeStoreButton.onClick.AddListener(GameManager.Instance.ResetGame);
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
