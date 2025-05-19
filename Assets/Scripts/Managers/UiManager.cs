using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Image _gameoverImage;
    [SerializeField] private Sprite _victorySprite;
    [SerializeField] private Sprite _defeatSprite;

    private void Start()
    {
        EventManager.instance.OnGameOver += OnGameOver;
        _gameoverImage.enabled = false;
    }

    private void OnGameOver(bool isVictory)
    { 
        Debug.Log("UiManager: OnGameOver: " + isVictory);
        _gameoverImage.sprite = isVictory ? _victorySprite : _defeatSprite;
        _gameoverImage.enabled = true;
    }
}
