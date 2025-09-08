using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelManager : SingletonBase<LevelManager>
{
    public LevelStats currentLevelStats;
    public PlayerStats currentPlayerStats;
    public DarkGStats currentDarkGStats;
    public TilemapStats currentTilemapStats;
    public Action CompleteLoading;
    
    protected override async void Awake()
    {
        base.Awake();
        await InitLevel();

    }

    public async Task InitLevel()
    {
        currentLevelStats = await AddressableUtility.LoadAssetAsync<LevelStats>(AddressableKey.LevelStats);
        currentPlayerStats = await AddressableUtility.LoadAssetAsync<PlayerStats>(AddressableKey.PlayerStats);
        currentTilemapStats = await AddressableUtility.LoadAssetAsync<TilemapStats>(AddressableKey.TilemapStats);
        currentDarkGStats = await AddressableUtility.LoadAssetAsync<DarkGStats>(AddressableKey.DarkGStats);
        int poolSize = currentLevelStats.enemyRatio * currentLevelStats.currentLevel;
        await ObjectPoolManager.Instance.CreateCreaturePool(AddressableKey.DarkGKey, poolSize);
        CompleteLoading?.Invoke();
        LevelStart();
        Debug.Log("Level Manager: Level Init Complete");
    }

    public void LevelStart()
    {
        for (int i = 0; i < currentLevelStats.enemyRatio * currentLevelStats.currentLevel; i++)
        {
            ObjectPoolManager.Instance.darkGPool.ActivePoolObject();
        }
    }

    private void OnDestroy()
    {
        CompleteLoading = null;
    }

}
