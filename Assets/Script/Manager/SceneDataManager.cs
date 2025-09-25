using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using static GlobalEventUtility;

public class SceneDataManager : SingletonBase<SceneDataManager>
{
    public LevelData currentLevelStats;
    public PlayerData currentPlayerStats;
    public DarkGData currentDarkGStats;
    public TilemapData currentTilemapStats;

    private bool m_loadingFinished = false;

    /// <summary>
    /// Game Data(Addressable) loading is completed (Invoke after Loading is finished)
    /// </summary>
    public event Action DataLoadingIsCompleted;

    /// <summary>
    /// All scene loading is completed (Invoke after Scene & Data is loaded)
    /// </summary>
    public event Action SceneIsReady;

    /// <summary>
    /// Change Loading State
    /// </summary>
    public event Action<LoadingState> ChangeLoadingAction;

    protected override void Awake()
    {
        base.Awake();
        SetDontDestroyOnLoad();
        ChangeSceneNotification += OnSceneChange;
        RestartSceneNotification += ReStartScene;

        StartLoadingNotification += OnStartLoading;
        IsLoadingNotification += OnIsLoading;
        FinishLoadingNotification += OnFinishLoading;

        //OnApplicationFirstStart();
    }

    private void Update()
    {
        
    }

    private async void OnSceneChange(string sceneName)
    {
        Coroutine sceneCoroutine= StartCoroutine(LoadSceneIEnumerator(sceneName));
        await LoadingDataAsync(LoadSceneLevelDataAsync());
    }
    private void ReStartScene()
    {
        SceneIsReady = null;
        DataLoadingIsCompleted = null;
    }
    private IEnumerator LoadSceneIEnumerator(string sceneName)
    {
        Debug.Log($"<color=cyan>[A1](frame= {Time.frameCount}) On Scene[{sceneName}] Start Loading...</color>");
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(sceneName);

        while (!sceneLoading.isDone || !m_loadingFinished)
            yield return null;

        Debug.Log($"<color=cyan>[A2](frame= {Time.frameCount}) On Scene[{sceneName}] Finish Loading...</color>");
        SceneIsReady?.Invoke();
    }
    private async Task LoadingDataAsync(Task mission = null)
    {
        StartLoadingNotification?.Invoke();


        if (mission != null)
        {
            IsLoadingNotification?.Invoke();
            await mission;
        }

        Task delay = Task.Delay(Minimum_LoadTime);
        await delay;

        FinishLoadingNotification?.Invoke();
    }

    public async Task LoadSceneLevelDataAsync()
    {
        currentLevelStats = await AddressableUtility.LoadAssetAsync<LevelData>(AddressableKey.LevelStats);
        currentPlayerStats = await AddressableUtility.LoadAssetAsync<PlayerData>(AddressableKey.PlayerStats);
        currentTilemapStats = await AddressableUtility.LoadAssetAsync<TilemapData>(AddressableKey.TilemapStats);
        currentDarkGStats = await AddressableUtility.LoadAssetAsync<DarkGData>(AddressableKey.DarkGStats);
    }

    #region Loading State Machine
    private void OnStartLoading()
    {
        ChangeLoadingAction?.Invoke(LoadingState.StartDataLoading);
        m_loadingFinished = false;
        Debug.Log($"<color=cyan>[B1](frame= {Time.frameCount}) On Start Loading...{m_loadingFinished}</color>");
    }

    private void OnIsLoading()
    {
        ChangeLoadingAction?.Invoke(LoadingState.IsDataLoading);
        Debug.Log($"<color=cyan>[B2](frame= {Time.frameCount}) On is Loading...{m_loadingFinished}</color>");
    }

    private void OnFinishLoading()
    {
        ChangeLoadingAction?.Invoke(LoadingState.FinishDataLoading);

        //Reset Loading State
        CurrentLoadingState = LoadingState.NoLoading;
        m_loadingFinished = true;
        DataLoadingIsCompleted?.Invoke();
        Debug.Log($"<color=cyan>[B3](frame= {Time.frameCount}) On Finish Loading...{m_loadingFinished}</color>");
    }
    #endregion

}
