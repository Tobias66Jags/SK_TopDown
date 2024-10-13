using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    PoolManager _poolManager;
    EnemySpawnManager _enemySpawnManager;
    ScoreManager _scoreManager;

    private Transform _playerTransform;
    private Vector3 _lookPos;
    private bool _canSpawn = true;

    public delegate void EnemyDeathEvent();
    public event EnemyDeathEvent OnEnemyDeath;

    public delegate void EnemyGetScore();
    public event EnemyGetScore OnEnemyGetScore; 

    [SerializeField] private float _speed = 1;

    [Header("Health Values")]
    [SerializeField] public int _maxHealth = 100;
    private int _currentHealth;
    [SerializeField] private float _delay = 2f;

    [Header("Attack Values")]
    private bool _canShoot = true;
    [SerializeField] private float _distanceToShoot = 5;
    [SerializeField] private float _distanceToFollow = 10;
    [SerializeField] private float _fireRate = 0.6f;
 
    [SerializeField] private Transform _bulletPos;

    [Header("Animation Values")]
    [SerializeField] Animator _enemyAnim;
    [SerializeField] private string _walkAnim;
    [SerializeField] private string _shootAnim;
    [SerializeField] private string _reloadAnim;
    [SerializeField] private string _deathAnim;


    public enum EnemyStates
    {
        Moving, Attacking, Dying
    }
    public EnemyStates currentState;

    private void Awake()
    {
        _poolManager = FindAnyObjectByType<PoolManager>();
        _enemyAnim = GetComponentInChildren<Animator>();
        _playerTransform = FindAnyObjectByType<PlayerController>().transform;
        _enemySpawnManager = FindAnyObjectByType<EnemySpawnManager>();
        _scoreManager = FindAnyObjectByType<ScoreManager>();

      InitializeEnemy();
    }

    private void OnEnable()
    {
        OnEnemyDeath += _enemySpawnManager.GetEliminatedEnemies;
        OnEnemyGetScore += _scoreManager.BeatEnemyScore;
    }


    private void OnDisable()
    {
        OnEnemyDeath -= _enemySpawnManager.GetEliminatedEnemies;
        OnEnemyGetScore -= _scoreManager.BeatEnemyScore;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyStates.Moving:

                Move();
                if (IsAttacking(_distanceToShoot)) currentState = EnemyStates.Attacking;
                break;
            case EnemyStates.Attacking:
                Attack();
               if (!IsAttacking(_distanceToFollow)) currentState = EnemyStates.Moving;

                break;
              
               
            case EnemyStates.Dying:
                Die();
                break;

            default: break;
        }
    }

    public void Move()
    {
        LookPlayer();
        _enemyAnim.Play(_walkAnim);
        transform.Translate(Vector3.forward*Time.deltaTime*_speed);
    }
    public void Attack() 
    {
        LookPlayer();
        _enemyAnim.Play(_shootAnim);
        if (_canShoot )
        {
            _canShoot = false;
           
            GameObject projectile = _poolManager.GetEnemyProjectile();
            projectile.transform.position = _bulletPos.position;
            projectile.transform.rotation = _bulletPos.rotation;
            projectile.SetActive(true);
            Invoke("SetShoot", _fireRate);
        }
    }

    public void SetShoot()
    {
        _canShoot = true;
    }
 
    public void Die()
    {
        _enemyAnim.Play(_deathAnim);
        SetReward();
        Invoke("Deactivate", _delay);
    }

    public void Deactivate()
    {
      
        InitializeEnemy();
        OnEnemyDeath?.Invoke();
        OnEnemyGetScore?.Invoke();
        gameObject.SetActive(false);
    }
    public void LookPlayer()
    {
        Vector3 pos = new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z);
        transform.LookAt(pos);
    }
    public void GetDamage(int damageValue)
    {
        _currentHealth -=damageValue;
        if (_currentHealth <= 0) currentState = EnemyStates.Dying;

    }

    private void InitializeEnemy()
    {
        _currentHealth = _maxHealth;
        _canSpawn = true;
        currentState = EnemyStates.Moving;
    } 

    public bool IsAttacking(float distance)
    {
       if (Vector3.Distance(transform.position, _playerTransform.position) <=distance)
        {
            return true;
        }else return false;
    }

    private void SetReward()
    {       
         if ( _canSpawn)
            {
             _canSpawn = false;
             float rand = Random.value;
            Debug.Log(rand);
          if (rand > 0.7)
            {
                
                GameObject newCoin = _poolManager.GetCoin();
                newCoin.transform.position = transform.position;
                newCoin.SetActive(true);
            }
            }else return;
               
    }
    }
