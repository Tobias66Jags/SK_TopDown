using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Rigidbody _rb;

 
    [SerializeField] private float _speed = 10f;

    [SerializeField] private int _damage = 10;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed, Space.Self);
    }


    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
           other.GetComponent<IDamageable>()?.GetDamage(_damage);
            Invoke("Deactivate", 0.1f);
        }
    }
}
