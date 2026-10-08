using System.Collections.Generic;
using UnityEngine;

public class SimplePool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int poolSize = 5;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);

            obj.SetActive(false);

            pool.Add(obj);
        }
    }

    public GameObject GetFromPool()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);

                Debug.Log("REUSED POOLED OBJECT: " + obj.name);

                return obj;
            }
        }

        Debug.Log("POOL EMPTY");

        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null)
            return;

        obj.SetActive(false);

        Debug.Log("RETURNED TO POOL: " + obj.name);
    }
}

