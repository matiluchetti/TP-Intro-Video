using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private int _shotCount = 5;
    [SerializeField] private AudioClip _emptyClip;
    private Animator anim;


    private void OnEnable() {
        if (anim == null)
            anim = GetComponent<Animator>();
            
        anim.ResetTrigger("Reload"); 
    }

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
                    transform.position + transform.forward * 0.5f * i,
                    Quaternion.identity);
                _currentBulletCount--;
            }

        }

        EventManager.instance.EventGunUpdate(this);

        if (_shotClip != null && _audioSource != null)
        {
            Debug.Log($"Disparo ejecutado. Clip asignado: {_shotClip != null}, AudioSource: {_audioSource != null}");
            _audioSource.PlayOneShot(_shotClip);
        }
    }

    public override void Reload() {
        anim.SetTrigger("Reload");
        base.Reload();
    }
}
