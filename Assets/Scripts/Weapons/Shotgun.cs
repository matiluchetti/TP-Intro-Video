using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private int _shotCount = 5;
    [SerializeField] private AudioClip _emptyClip;

    public override void Attack()
    {
        if (_shotCount <= 0 || _currentBulletCount <= 0)
        {
            Debug.Log("No hay balas disponibles.");
            if (_emptyClip != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(_emptyClip);
            }
            return;
        }
        for (int i = 0; i < _shotCount && _currentBulletCount > 0; i++)
        {
            if (_currentBulletCount <= 0)
            {
                Debug.Log("No hay balas disponibles.");

            }
            else
            {
                Instantiate(
                    BulletPrefab,
                    transform.position + Random.insideUnitSphere * 1,
                    Quaternion.identity);
                _currentBulletCount--;
            }

        }

        if (_shotClip != null && _audioSource != null)
        {
            Debug.Log($"Disparo ejecutado. Clip asignado: {_shotClip != null}, AudioSource: {_audioSource != null}");
            _audioSource.PlayOneShot(_shotClip);
        }
    }

    public override void Reload() => base.Reload();
}
