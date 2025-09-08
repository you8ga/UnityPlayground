using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ObjectPoolManager : SingletonBase<ObjectPoolManager>
{
    public ObjectPoolBase darkGPool;

    protected override void Awake()
    {
        base.Awake();
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
    }
}
