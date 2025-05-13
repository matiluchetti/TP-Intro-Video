using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    static public EventManager instance;

    #region UNITY_EVENTS
    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
    }
    #endregion

    #region GAME_MANAGER
    public event Action<bool> OnGameOver;

    public void EventGameOver(bool isVictory) 
    {
        if (OnGameOver != null) OnGameOver(isVictory);
    }
    #endregion
}