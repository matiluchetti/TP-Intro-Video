using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;

    private bool isVictory;

    void Start()
    {
        isVictory = GameState.IsVictory;

        Debug.Log("isVictory: " + isVictory);

        gameOverText.text = isVictory ? "Victory!" : "Defeat!";

        SoundManager.Instance?.PlayGameOverSound(isVictory);
    }
}

