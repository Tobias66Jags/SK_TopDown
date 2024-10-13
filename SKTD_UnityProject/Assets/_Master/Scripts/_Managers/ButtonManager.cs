using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(() => { SceneFlowManager.Instance.LoadLevel("SampleScene"); });
        _playButton.onClick.AddListener(GameManager.Instance.ResetGame);
    }
}
