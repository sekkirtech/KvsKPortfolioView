using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPoolManager : MonoBehaviour
{
    ObjectPool<GameObject> pool;

    public GameObject Prefab { get; private set; }

    void Awake()
    {
        pool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    GameObject OnCreatePooledObject()
    {
        return Instantiate(Prefab);
        
    }

    void OnGetFromPool(GameObject obj)
    {
        
        obj.SetActive(true);
    }

    void OnReleaseToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetGameObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        Prefab = prefab;
        GameObject obj = pool.Get();
        Transform tf = obj.transform;
        tf.position = position;
        tf.rotation = rotation;

        return obj;
    }

    public void ReleaseGameObject(GameObject obj)
    {
        pool.Release(obj);
    }
}
