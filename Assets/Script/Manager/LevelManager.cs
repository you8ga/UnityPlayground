using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonBase<LevelManager>
{
    private LevelStats m_stats;

    protected override async void Awake()
    {
        base.Awake();
        m_stats = await AddressableUtility.LoadAssetAsync<LevelStats>(AddressableKey.LevelStats);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
