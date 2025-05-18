using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{

    [SerializeField] private AudioClip _emptyClip;
    
    public override void Attack()
    {
        if (_currentBulletCount <= 0)
        {
            Debug.Log("No hay balas disponibles.");
            if (_emptyClip != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_emptyClip);
            }
            return;
        }

        Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        _currentBulletCount--;

        if (_shotClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_shotClip);
        }
    }

    public override void Reload() => base.Reload();
}
