using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressableUtility
{
    public static Dictionary<string, object> loadedAssets = new Dictionary<string, object>();

    public static async Task<T> LoadAssetAsync<T>(string address)
    {
        Debug.Log($"LoadAsset by Addressable Key: {address}");

        if (loadedAssets.ContainsKey(address))
            return (T)loadedAssets[address];

        var asyncHandle = Addressables.LoadAssetAsync<T>(address);
        await asyncHandle.Task;

        if (asyncHandle.Status == AsyncOperationStatus.Succeeded)
        {
            T asset = asyncHandle.Result;
            loadedAssets[address] = asset;
            return asset;
        }

        Debug.LogError($"Failed to load asset at address: {address}");
        return default(T);
    }

    public static object GetAssetData(string className)
    {
        return loadedAssets[className];
    }

    //private static void OnDestroy()
    //{
    //    foreach (var asset in loadedAssets.Values)
    //    {
    //        Addressables.Release(asset);
    //    }
    //    loadedAssets.Clear();
    //}
}
