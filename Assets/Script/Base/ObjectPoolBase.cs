using System.Collections.Generic;
using UnityEngine;
public abstract class ObjectPoolBase<T> where T : Component
{
    public T prefab;
    public Queue<T> poolObjects = new Queue<T>();

    public ObjectPoolBase(T prefab, int poolSize)
    {
        this.prefab = prefab;
        for (int i = 0; i < poolSize; i++)
        {
            poolObjects.Enqueue(InitObject(prefab));
        }
    }

    public virtual T InitObject(T prefab)
    {
        T newObject = GameObject.Instantiate(prefab);
        newObject.gameObject.SetActive(false);
        return newObject;
    }

    // �q��������������
    public T GetPoolObject()
    {
        if (poolObjects.Count == 0)
        {
            return null;
        }

        T objectToGet = poolObjects.Dequeue();
        objectToGet.gameObject.SetActive(true);
        return objectToGet;
    }

    // �N�����k�٨쪫���
    public void ReturnObjectToPool(T objectToReturn)
    {
        objectToReturn.gameObject.SetActive(false);
        poolObjects.Enqueue(objectToReturn);
    }
}
