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

    #region DAMAGEABLES
    public event Action<int,int> OnLifeUpdate;

    public void EventLifeUpdate(int currentLife, int maxLife)
    {
        if (OnLifeUpdate != null) OnLifeUpdate(currentLife, maxLife);
    }
    #endregion

    #region GUN
    public event Action<Gun> OnGunUpdate;
    public void EventGunUpdate(Gun gun)
    {
        if (OnGunUpdate != null) OnGunUpdate(gun);
    }
    #endregion

    #region ROUND

    public event Action<int> OnRoundUpdate;

    public void EventRoundUpdate(int round)
    {
        if (OnRoundUpdate != null) OnRoundUpdate(round);
    }
    #endregion

}