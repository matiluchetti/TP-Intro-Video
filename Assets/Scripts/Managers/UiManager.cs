using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{


    #region LIFE_UPDATE
    [Header("Life Update")]
    [SerializeField] private Image _lifeBar;

    private void InitLifeUpdater()
    {
        EventManager.instance.OnLifeUpdate += OnLifeUpdate;
        _lifeBar.fillAmount = 1f; // Inicializa la barra de vida al 100%
    }


    private void OnLifeUpdate(int currentLife, int maxLife)
    {
        float lifePercentage = (float)currentLife / maxLife;
        _lifeBar.fillAmount = lifePercentage;

        if (lifePercentage <= .3f){
            _lifeBar.color = Color.red;
        } else if (lifePercentage <= .6f){
            _lifeBar.color = Color.yellow;
        } else {
            _lifeBar.color = Color.green;
        }
    }

    #endregion

    #region GUN_BULLET
    [Header("Gun Bullet")]
    [SerializeField] private TMP_Text _bulletText;
    [SerializeField] private Image gunImage;
    [SerializeField] private TMP_Text roundText;

    private void InitGunUpdater()
    {
        EventManager.instance.OnGunUpdate += OnGunUpdate;

    }

    private void OnGunUpdate(Gun gun)
    {
        _bulletText.text = $"{gun.CurrentBullets}/{gun.MaxBulletCount}"; // Actualiza el texto de las balas
        gunImage.sprite = gun.GunSprite;
    }


    private void InitRoundUpdater()
    {
        EventManager.instance.OnRoundUpdate += OnRoundUpdate;
        roundText.text = "Oleada 1"; // Inicializa el texto de la ronda
    }

    private void OnRoundUpdate(int round)
    {
        roundText.text = $"Oleada {round}";
    }

    #endregion


    private void Start()
    {
        InitLifeUpdater();
        InitGunUpdater();
        InitRoundUpdater();
    }
}
