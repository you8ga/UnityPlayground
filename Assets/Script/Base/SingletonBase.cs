using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
                CreateInstance();
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    private static void CreateInstance()//if doesn't have InputManager Object, create a new instance
    {
        GameObject singletonObject = new GameObject();
        _instance = singletonObject.AddComponent<T>();
        singletonObject.name = typeof(T).ToString() + " (Singleton)";
        DontDestroyOnLoad(singletonObject);
    }
}
