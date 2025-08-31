using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : SingletonBase<AddressableManager>
{
    public List<string> allAddresses;
    private Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();

    protected override void Awake()
    {
        base.Awake();
    }

    public async Task<T> LoadAssetAsync<T>(string address)
    {
        Debug.Log($"LoadAsset by Addressable Key: {address}");

        if (_loadedAssets.ContainsKey(address))
            return (T)_loadedAssets[address];

        var asyncHandle = Addressables.LoadAssetAsync<T>(address);
        await asyncHandle.Task;

        if (asyncHandle.Status == AsyncOperationStatus.Succeeded)
        {
            T asset = asyncHandle.Result;
            _loadedAssets[address] = asset;
            return asset;
        }

        Debug.LogError($"Failed to load asset at address: {address}");
        return default(T);
    }

    public object GetAssetData(string className)
    {
        return _loadedAssets[className];
    }

    private void OnDestroy()
    {
        foreach (var asset in _loadedAssets.Values)
        {
            Addressables.Release(asset);
        }
        _loadedAssets.Clear();
    }
}
