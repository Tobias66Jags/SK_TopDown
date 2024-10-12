using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;


    [Header("Canvas")]
    [SerializeField] private Canvas _loadingScreen;

    [Header("Screens")]

    [Header("Slider")]
    [SerializeField] private Slider _loadingSlider;

    private void OnEnable()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        #endregion 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    public void LoadLevel(string level)
    {
        _loadingScreen.enabled = true;
        StartCoroutine(LoadLevelAsync(level));
    }

    IEnumerator LoadLevelAsync(string level)
    {

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(level);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            _loadingSlider.value = progressValue;
            yield return null;
        }
        _loadingScreen.enabled = false;


    }

    [ContextMenu("Scene Test")]
    public void TestScene()
    {
        LoadLevel("PlayerProves");
    }

    [ContextMenu("Back to menu")]
    public void TestMenu()
    {
        LoadLevel("MainMenu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        switch (scene.buildIndex)
        {
            case 0:
                break;
            case 1:
                Debug.Log("funciona el evento de escenas");
                
                break;
            case 3:

                break;
            case 2:

                break;
            case 4:

                break;
        }
    }
}
