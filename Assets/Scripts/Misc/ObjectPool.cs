using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject objectToPool;
    readonly List<GameObject> pooledObjects = new List<GameObject>();
    
    public GameObject GetObject()
    {
        if (objectToPool == null) return null;
        for (int i = 0; i < pooledObjects.Count; i++)
            if (!pooledObjects[i].activeSelf)
                return pooledObjects[i];

        GameObject newObject = Instantiate(objectToPool, transform);
        newObject.SetActive(false);
        pooledObjects.Add(newObject);
        return newObject;
    }
}
