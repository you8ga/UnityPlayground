using System.Collections.Generic;
using UnityEngine;
public class ObjectPoolBase
{
    public GameObject prefab;
    public Queue<GameObject> poolObjects = new Queue<GameObject>();

    public ObjectPoolBase(Transform parent, GameObject prefab, int poolSize)
    {
        this.prefab = prefab;
        for (int i = 0; i < poolSize; i++)
        {
            poolObjects.Enqueue(InitObject(parent,prefab));
        }
    }

    public virtual GameObject InitObject(Transform parent, GameObject prefab)
    {
        GameObject newObject = GameObject.Instantiate(prefab);
        newObject.transform.SetParent(parent);
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    // 從物件池中獲取物件
    public GameObject GetPoolObject()
    {
        if (poolObjects.Count == 0)
        {
            return null;
        }

        GameObject objectToGet = poolObjects.Dequeue();
        objectToGet.gameObject.SetActive(true);
        return objectToGet;
    }

    // 將物件歸還到物件池
    public void ReturnObjectToPool(GameObject objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        poolObjects.Enqueue(objectToReturn);
    }
}
