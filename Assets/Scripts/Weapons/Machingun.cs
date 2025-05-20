using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machingun : Gun
{
    [SerializeField] private int _shotCount = 5;
    [SerializeField] private AudioClip _emptyClip;

    public Machingun()
    {
        _maxBulletCount = 60; // Cambia el valor al crear la instancia
    }

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

        int actualShotsFired = 0;

        for (int i = 0; i < _shotCount; i++)
        {
            if (_currentBulletCount <= 0)
            {
                Debug.Log("Sin balas a mitad de ráfaga.");
                break;
            }

            Instantiate(
                BulletPrefab,
                transform.position + transform.forward * i * 0.6f ,
                transform.rotation);

            _currentBulletCount--;
            actualShotsFired++;
        }
        EventManager.instance.EventGunUpdate(this); // Actualiza el UI

        if (actualShotsFired > 0 && _shotClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_shotClip);
        }
    }

    public override void Reload() => base.Reload();
}
