using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;

    PlayerController _playerController;

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

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }



    public void GameOver()
    {
        isPlay = false;
    }
}



