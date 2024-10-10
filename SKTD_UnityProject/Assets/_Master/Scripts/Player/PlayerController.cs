using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerActions _playerInput;
    private InputAction _moveAction;
    private InputAction _lightShootAction;
    private InputAction _heavyShootAction;

    private CharacterController _characterController;
    private Vector3 _velocity;
    private Vector3 _animValue;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _turnSmoothTime = 0.5f;

    [Header("Anim Settings")]
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
      

        if (_playerInput == null)
        {
            _playerInput = new PlayerActions();
        }

        _moveAction = _playerInput.MovingActions.Movement; 
        _lightShootAction = _playerInput.ShootingActions.PrimaryShoot; 
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleAiming();
        HandleShooting();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        moveDirection = moveDirection.normalized;

     
        _velocity.x = moveDirection.x * _moveSpeed;
        _velocity.z = moveDirection.z * _moveSpeed;

        _characterController.Move(_velocity * Time.deltaTime);

       // moveDirection = transform.TransformDirection(moveDirection).normalized;

        Vector3 setAnim = transform.TransformDirection(moveDirection).normalized;

        _animator.SetFloat("Horizontal", setAnim.x);
        _animator.SetFloat("Vertical", setAnim.z);

        Debug.Log(setAnim);
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
    private void HandleShooting()
    {
        if (_lightShootAction.IsPressed())
        {
            _animator.SetLayerWeight(1, 1);
        }
        else
        {
            _animator.SetLayerWeight(1, 0);
        }
    }

}
