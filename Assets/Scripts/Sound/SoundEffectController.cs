using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectController : MonoBehaviour, IListenable
{
    #region IListenable_Properties
    public AudioClip AudioClip => _audioClip;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioClip _victoryAudioClip;
    [SerializeField] private AudioClip _defeatAudioClip;

    public AudioSource AudioSource => _audioSource;
    private AudioSource _audioSource;
    #endregion

    #region IListenable_Methods
    public void InitAudioSource()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = AudioClip;
    }

    public void PlayOnShot() => AudioSource.PlayOneShot(AudioClip);

    public void Play() => AudioSource.Play();

    public void Stop() => AudioSource.Stop();
    #endregion

    #region Unity_Events
    void Start()
    {
        InitAudioSource();
        EventManager.instance.OnGameOver += OnGameOver;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) PlayOnShot();
        if (Input.GetKeyDown(KeyCode.P)) Play();
    }
    #endregion

    #region EVENT_ACTIONS
    private void OnGameOver(bool isVictory)
    {
        if (isVictory)  AudioSource.PlayOneShot(_victoryAudioClip);
        else            AudioSource.PlayOneShot(_defeatAudioClip);
    }
    #endregion
}
