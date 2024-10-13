using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Missil : MonoBehaviour
{
    Rigidbody _rb;

    [SerializeField] private ParticleSystem _explosionEffect;
    [SerializeField] private float _explosionRadius = 5f;  
    [SerializeField] private float _explosionForce = 700f;  

    [SerializeField] private float _delay = 0.2f;  
    [SerializeField] private float _speed = 10f;  

    [SerializeField] private int _damage = 100;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
      transform.Translate(Vector3.forward*Time.deltaTime*_speed, Space.Self);
    }

    void Explode()
    {
       
        _explosionEffect.Play();  

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
           
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            //Debug.Log(nearbyObject.gameObject.name);  
            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
            if(nearbyObject.gameObject.name != "Player") nearbyObject.GetComponent<IDamageable>()?.GetDamage( _damage);

        }
       
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other)
        {
            Explode();
            Invoke("Deactivate", _delay);
        }
    }
}
