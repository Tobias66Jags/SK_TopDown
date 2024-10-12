using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeapon : MonoBehaviour
{
   
    private bool _canShoot = true;
   

    [Header("Attack Values")]
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _damage;
    [SerializeField] private float _forceMultiplier;
    [SerializeField] private float _fireRate = 0.5f;


    [Header("Sound Values")]
    [SerializeField] private string _hitEventName = "Punch";
    [SerializeField] private string _hitSoundName = "Fist";
   

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _hitEffect;


    public void Shoot()
    {
        if (_canShoot)
        {
            _canShoot = false;

            _muzzleFlash?.Play();

            //EventManager.TriggerEvent(_hitEventName, property: _hitSoundName);


            Vector3 direction = transform.forward;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, _attackDistance))
            {
                Debug.DrawRay(transform.position + _offset, direction * _attackDistance, Color.red, 2f);
                Debug.Log(hit.transform.name);

                 hit.collider.GetComponent<IDamageable>()?.GetDamage(null, _damage);
                _hitEffect.transform.position = hit.point;
                _hitEffect.Play();
            }

            Invoke("ResetBool", _fireRate);

        }
    }
  

    public void ResetBool()
    {
        _canShoot = true;
    }

}
