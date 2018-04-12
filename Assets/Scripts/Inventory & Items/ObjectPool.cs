using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[System.Serializable]
public struct PooledObject
{
    public GameObject m_object;
    public int m_amount;
}
public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private PooledObject m_pooledObject;

    [SerializeField]
    private bool m_willGrow = true;

    [SerializeField]
    private List<GameObject> m_pool = new List<GameObject>();


    void Awake()
    {
        for (int i = 0; i < m_pooledObject.m_amount; i++)
        {
            GameObject obj = Instantiate(m_pooledObject.m_object);
            obj.SetActive(false);
            m_pool.Add(obj);
  

        }

    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_pool.Clear();
        for (int i = 0; i < m_pooledObject.m_amount; i++)
        {
            GameObject obj = Instantiate(m_pooledObject.m_object);
            obj.SetActive(false);
            m_pool.Add(obj);

        }
    }


    public GameObject GetPooledObject()
    {
        for (int i = 0; i < m_pool.Count; i++)
        {
            if (!m_pool[i].activeInHierarchy)
            {
                return m_pool[i];
            }
        }

        if (m_willGrow)
        {
            GameObject obj;
            obj = Instantiate(m_pooledObject.m_object) as GameObject;
            m_pool.Add(obj);


            return obj;
        }

        return null;
    }

    void OnDestroy()
    {
        for (int i = 0; i < m_pool.Count; i++)
        {
            Destroy(m_pool[i]);
        }
        m_pool.Clear();
    }
}