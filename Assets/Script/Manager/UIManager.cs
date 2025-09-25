using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalEventUtility;

public class UIManager : SingletonBase<UIManager>
{
    public GameObject mainMenuUI;
    public GameObject gamePlayingUI;
    public GameObject gamePhaseUI;
    public GameObject gameOverUI;
    public GameObject loadingUI;

    public Button startButton;
    public Button pauseButton;
    public Button settingButton;
    public Button restartButton;
    public Button quitButton;

    private SceneTimer m_UIdelayTimer;
    private Camera m_UICamera;
    private Canvas m_canvas
    {
        get
        {
            if (_canvas == null)
                _canvas = GetComponent<Canvas>();
            return _canvas;
        }
    }
    private Canvas _canvas;
    public enum UIState
    {
        MainMenu,
        GamePlaying,
        GamePhase,
        GameOver,
        Setting
    }

    private UIState m_currentUIState;
    protected override void Awake()
    {
        base.Awake();
        SetDontDestroyOnLoad();
        SetCamera();
        DontDestroyOnLoad(m_UICamera);

        startButton.onClick.AddListener(OnStartButtonClicked);
        //pauseButton.onClick.AddListener(OnPauseButtonClicked);
        //settingButton.onClick.AddListener(OnSettingButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);

        SceneDataManager.Instance.ChangeLoadingAction += ActiveLoadingUI;
        GlobalEventUtility.GameOverNotification += OnGameOverUI;
        SwitchUIView(UIState.MainMenu);
    }

    private void SetCamera()
    {
        m_UICamera = m_canvas.worldCamera;
    }
    private void OnStartButtonClicked()
    {
        ChangeSceneNotification?.Invoke(SceneKey.GameScene);
        SwitchUIView(UIState.GamePlaying);
    }

    private void OnPauseButtonClicked()
    {

    }

    private void OnSettingButtonClicked()
    {

    }

    private void OnRestartButtonClicked()
    {
        RestartSceneNotification?.Invoke();
        ChangeSceneNotification?.Invoke(SceneKey.GameScene);
        SwitchUIView(UIState.GamePlaying);
    }
    private void OnQuitButtonClicked()
    {
        Debug.Log("OnQuitButtonClicked");
    }
    private void OnGameOverUI()
    {
        SwitchUIView(UIState.GameOver);
    }
    
    private void ActiveLoadingUI(LoadingState loading)
    {
        if(loading == LoadingState.NoLoading || loading == LoadingState.FinishDataLoading)
            loadingUI.SetActive(false);
        else
            loadingUI.SetActive(true);
    }
    
    private void SwitchUIView(UIState state)
    {
        mainMenuUI.SetActive(state == UIState.MainMenu);
        gameOverUI.SetActive(state == UIState.GameOver);
    }

}
