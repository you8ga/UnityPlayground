using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static GlobalEventUtility;

public class ObjectPoolManager : SingletonBase<ObjectPoolManager>
{
    public ObjectPoolBase darkGPool;
    private LevelData m_currentLevelStats;
    protected override void Awake()
    {
        base.Awake();
        SceneDataManager.Instance.DataLoadingIsCompleted += () => m_currentLevelStats = SceneDataManager.Instance.currentLevelStats;
        SceneDataManager.Instance.SceneIsReady +=InitPool;
    }


    private async void InitPool()
    {
        int poolSize = m_currentLevelStats.enemyRatio * m_currentLevelStats.currentLevel;
        await CreateCreaturePool(AddressableKey.DarkGKey, poolSize);
    }
    public async Task CreateCreaturePool(string key,int size)
    {
        GameObject poolParent = new GameObject();
        poolParent.name = $"{key} Pool";
        GameObject loadedObj = await AddressableUtility.LoadAssetAsync<GameObject>(key);
        if (loadedObj !=null)
        {
            darkGPool = new ObjectPoolBase(poolParent.transform, loadedObj, size);
        }

        ActivePoolObjects();
    }

    private void ActivePoolObjects()
    {
        for (int i = 0; i < m_currentLevelStats.enemyRatio * m_currentLevelStats.currentLevel; i++)
        {
            darkGPool.ActivePoolObject();
        }
        Debug.Log($"<color=green>(frame= {Time.frameCount}) Active [{m_currentLevelStats.enemyRatio * m_currentLevelStats.currentLevel}] of Pool Objects...</color>");
    }
}
