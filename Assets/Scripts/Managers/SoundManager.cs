using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _defeatClip;
    [SerializeField] private AudioClip _playerHitClip; // Asigná el SFX de daño en el inspector
    [SerializeField] private AudioClip _waveEndClip; // Asigná el SFX en el inspector

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Character.OnPlayerDamaged += OnPlayerDamaged;
        EventManager.instance.OnGameOver += OnGameOver;
        WaveManager.OnWaveEnded += OnWaveEnded; // Suscribite
    }

    void OnDisable()
    {
        Character.OnPlayerDamaged -= OnPlayerDamaged;
        EventManager.instance.OnGameOver -= OnGameOver;
        WaveManager.OnWaveEnded -= OnWaveEnded; // Desuscribite
    }

    private void OnWaveEnded()
    {
            Debug.Log("OnWaveEnded llamado");
        if (_waveEndClip != null && _audioSource != null)
            _audioSource.PlayOneShot(_waveEndClip);
    }

    private void OnGameOver(bool isVictory)
    {
        if (_audioSource != null)
            _audioSource.PlayOneShot(isVictory ? _victoryClip : _defeatClip);
    }

    private void OnPlayerDamaged(float amount)
    {
        if (_playerHitClip != null && _audioSource != null)
            _audioSource.PlayOneShot(_playerHitClip);
    }
}