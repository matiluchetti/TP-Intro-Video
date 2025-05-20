using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LoadingController _loadingController;

    private void Start()
    {
        EventManager.instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver(bool isVictory)
    {
        Debug.Log("GameManager: OnGameOver: " + isVictory);
        GameState.IsVictory = isVictory;
        GameState.IsGameOver = true;
        
        if (_loadingController != null)
        {
            _loadingController.LoadScene(2);
        }
        else
        {
            Debug.LogError("LoadingController reference is missing in GameManager!");
        }
    }
}
