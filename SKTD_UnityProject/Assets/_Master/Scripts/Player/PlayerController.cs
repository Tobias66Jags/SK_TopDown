using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamageable
{
    private PlayerActions _playerInput;
    private InputAction _moveAction;
    private InputAction _lightShootAction;
    private InputAction _heavyShootAction;

    private RayCastWeapon _weapon;
    private PooledWeapon _pooledWeapon;

    [SerializeField] private Transform _shootPosition;

    private CharacterController _characterController;
    private Vector3 _velocity;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _turnSmoothTime = 0.5f;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int maxShield = 100;
    public int currentHealth;
    public int currentShield;

    public float timeToRegenerateShield = 5;
    public float timeBetweenShield = 0.5f;

    public delegate void HealthUpdateEvent();
    public event HealthUpdateEvent OnHealthUpdated;

    public delegate void DeathEvent();
    public event DeathEvent OnDeath;

    [Header("Anim Settings")]
    [SerializeField] private Animator _animator;

    public int shieldRegenerationRate = 10; 
    public float shieldRegenerationInterval = 0.5f; 
    public float timeToStartRegeneration = 5f; 
    private float timeSinceLastDamage = 0f; 
    private bool isRegenerating = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _weapon = GetComponentInChildren<RayCastWeapon>();
        _pooledWeapon = GetComponentInChildren<PooledWeapon>();

        if (_playerInput == null)
        {
            _playerInput = new PlayerActions();
        }

        _moveAction = _playerInput.MovingActions.Movement;
        _lightShootAction = _playerInput.ShootingActions.PrimaryShoot;
        _heavyShootAction = _playerInput.ShootingActions.SecondaryShoot;

        currentHealth = maxHealth;
        currentShield = maxShield;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        OnDeath += GameManager.Instance.GameOver;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        OnDeath -= GameManager.Instance.GameOver;
    }

    private void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            HandleMovement();
            HandleAiming();
            HandleRegularShooting();
            HandleMissilShooting();



            if (!isRegenerating)
            {
                timeSinceLastDamage += Time.deltaTime;


                if (timeSinceLastDamage >= timeToStartRegeneration)
                {
                    StartCoroutine(RegenerateShield());
                }
            }

        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        _velocity.x = moveDirection.x * _moveSpeed;
        _velocity.z = moveDirection.z * _moveSpeed;

        _characterController.Move(_velocity * Time.deltaTime);

        Vector3 setAnim = transform.TransformDirection(moveDirection).normalized;
        _animator.SetFloat("Horizontal", setAnim.x);
        _animator.SetFloat("Vertical", setAnim.z);
    }

    private void HandleAiming()
    {
        Ray pointRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 lookDir = worldMousePos - transform.position;
        lookDir.y = 0f;

        if (Physics.Raycast(pointRay, out hit))
        {
            Vector3 mouseDirection = hit.point - transform.position;
            mouseDirection.y = 0f;

            Quaternion rotateToMouse = Quaternion.LookRotation(mouseDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToMouse, _turnSmoothTime);
        }

        Debug.DrawRay(pointRay.origin, pointRay.direction * 100f, Color.red);
    }

    private void HandleRegularShooting()
    {
        if (_lightShootAction.IsPressed())
        {
            _animator.SetLayerWeight(1, 1);
            _weapon.Shoot();
        }
        else
        {
            _animator.SetLayerWeight(1, 0);
        }
    }

    private void HandleMissilShooting()
    {
        if (_heavyShootAction.triggered)
        {
            _animator.SetLayerWeight(2, 1);
            _pooledWeapon.ShootMissile(_shootPosition);
        }
        else
        {
            _animator.SetLayerWeight(2, 0);
        }
    }

    [ContextMenu("Get Hurt")]
    public void TryHurt()
    {
        GetDamage(50);
    }

    public void GetDamage(int damageAmount)
    {
      
        
        if (currentShield > 0)
        {
            currentShield -= damageAmount*2;
           
        }
        else
        {
         
            currentHealth -= damageAmount;
            if (currentHealth <= 0) OnDeath?.Invoke();
        }

    
        timeSinceLastDamage = 0f;

    
        if (isRegenerating)
        {
            StopAllCoroutines(); 
            isRegenerating = false;
        }

        CallHealthUpdate();

    }

    public void CallHealthUpdate()
    {
        OnHealthUpdated?.Invoke();
    }

    IEnumerator RegenerateShield()
    {
        isRegenerating = true;

        while (currentShield < maxShield)
        {
            currentShield += shieldRegenerationRate;
            currentShield = Mathf.Min(currentShield, maxShield); 

            CallHealthUpdate();

            yield return new WaitForSecondsRealtime(shieldRegenerationInterval); 
        }

        isRegenerating = false; 
    }
}
