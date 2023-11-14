using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Serializable]
    public class ObjectPool
    {
        public GameObject poolGameObject;
        public int initialSize;
    }

    public static ObjectPoolManager instance;

    public List<ObjectPool> pools = new List<ObjectPool>();
    private Dictionary<string, Queue<GameObject>> poolDict = new();

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitializeObjectPools();        
    }

    private void InitializeObjectPools()
    {
        foreach(var pool in pools)
        {
            Queue<GameObject> objectQueue = new();
            for (int i = 0; i < pool.initialSize; i++)
            {
                GameObject obj = Instantiate(pool.poolGameObject);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            poolDict.Add(pool.poolGameObject.name, objectQueue);
        }
    }

    public GameObject SpawnFromPool(string prefabName, Vector3 position, Quaternion rotation)
    {
        if(!poolDict.ContainsKey(prefabName))
        {
            Debug.LogWarning($"Object with name {prefabName} not found");
            return null;

        }

        GameObject objectToSpawn = poolDict[prefabName].Dequeue();

        if(objectToSpawn != null)
        {
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }

        poolDict[prefabName].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
