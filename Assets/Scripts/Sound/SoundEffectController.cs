using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Agregamos un componente obligatorio -> Esto fueza a que unity agregue 
// el componente si no existe en el objeto.
[RequireComponent(typeof(AudioSource))]
public class SoundEffectController : MonoBehaviour, IListenable
{
    #region IListenable_Properties
    // El audio quedará asignado por inspector
    public AudioClip AudioClip => _audioClip;
    /// SerializeField nos permite exponer una propiedad privada en el inspector
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private AudioClip _victoryAudioClip;
    [SerializeField] private AudioClip _defeatAudioClip;

    public AudioSource AudioSource => _audioSource;
    private AudioSource _audioSource;
    #endregion

    #region IListenable_Methods
    public void InitAudioSource()
    {
        // Asignar el componente AudioSource
        _audioSource = GetComponent<AudioSource>();
        // Asignamos el audio clip al AudioSource
        _audioSource.clip = AudioClip;
    }

    // Reproducir de esta manera evita tener que asignar un clip al source
    public void PlayOnShot() => AudioSource.PlayOneShot(AudioClip);

    // Esta reproducción necesita tener que asignar un clip al source
    public void Play() => AudioSource.Play();

    // Detiene un clip si esta asignado y en reproducción
    public void Stop() => AudioSource.Stop();
    #endregion

    #region Unity_Events
    // Start is called before the first frame update
    void Start()
    {
        InitAudioSource();
        EventManager.instance.OnGameOver += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        // Al presionar una tecla le damos play al audio clip
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
