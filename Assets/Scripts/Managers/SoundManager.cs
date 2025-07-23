using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _defeatClip;
    [SerializeField] private AudioClip _playerHitClip;
    [SerializeField] private AudioClip _waveEndClip;


    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        Character.OnPlayerDamaged += OnPlayerDamaged;
        if (EventManager.instance != null)
        {
            EventManager.instance.OnGameOver += OnGameOver;
        }
        WaveManager.OnWaveEnded += OnWaveEnded; 
    }

    void OnDisable()
    {
        Character.OnPlayerDamaged -= OnPlayerDamaged;
        if (EventManager.instance != null)
        {
            EventManager.instance.OnGameOver -= OnGameOver;
        }
        WaveManager.OnWaveEnded -= OnWaveEnded; 
    }

    private void OnWaveEnded()
    {
        Debug.Log("OnWaveEnded llamado");
        if (!GameState.IsGameOver && _waveEndClip != null && _audioSource != null)
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

    public void PlayGameOverSound(bool isVictory)
{
    if (_audioSource != null)
    {
        _audioSource.PlayOneShot(isVictory ? _victoryClip : _defeatClip);
    }
}

}