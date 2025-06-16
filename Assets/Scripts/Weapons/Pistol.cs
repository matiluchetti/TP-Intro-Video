using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{

    [SerializeField] private AudioClip _emptyClip;
    private Animator anim;
    

    public Pistol()
    {
        _maxBulletCount = 20; // Cambia el valor al crear la instancia
    }

    public void OnEnable(){
        if (anim == null)
            anim = GetComponent<Animator>();
            
        anim.ResetTrigger("Reload"); 
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

        Instantiate(BulletPrefab, transform.position, Quaternion.identity);
        _currentBulletCount--;

        EventManager.instance.EventGunUpdate(this); // Actualiza el UI

        if (_shotClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(_shotClip);
        }
    }

    public override void Reload() { 
        anim.SetTrigger("Reload");
        base.Reload();
    }
}
