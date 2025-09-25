using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 通知全局事件
/// </summary>
public static class GlobalEventUtility
{
    public enum LoadingState
    {
        //CheckUpdate will be added later
        NoLoading,
        SceneLoading,
        StartDataLoading,
        IsDataLoading,
        FinishDataLoading,
    }
    /// <summary>
    /// Callback for scene change, parameters: sceneName
    /// </summary>
    public static Action<string> ChangeSceneNotification;
    public static Action RestartSceneNotification;
    public static LoadingState CurrentLoadingState;

    public const int Minimum_LoadTime = 1000;
    public static Action StartLoadingNotification;
    public static Action IsLoadingNotification;
    public static Action FinishLoadingNotification;

    public static Action GameOverNotification;

}
