using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ScoreManager;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
 
    PlayerController _playerController;
    

    public delegate void RoundChanged();
    public event RoundChanged OnRoundChanged;

    public delegate void RoundFinished();
    public event RoundFinished OnRoundFinished;

    public int roundCounter = 1;
    public bool isPlay = false;

    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        #endregion

    }

    public void ResetRounds()
    {
        roundCounter = 1;
        OnRoundChanged?.Invoke();
    }

    public void IncreaseRound()
    {
        roundCounter++;
        if (roundCounter % 3 == 0)
        {
            RayCastWeapon weapon = FindAnyObjectByType<RayCastWeapon>();
            weapon._damage = Mathf.Max(4, weapon._damage - 1);
        }

        OnRoundChanged?.Invoke();
    }

    public void FinishRound()
    {
        isPlay = false;
        AudioManager.Instance.PlaySound("EndRound");
        OnRoundFinished?.Invoke();
    }

    public void ResetGame()
    {
        isPlay = true;
    }

    public void GameOver()
    {
        isPlay = false;
    }
}



