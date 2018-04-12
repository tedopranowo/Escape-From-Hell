using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
struct RandObj
{
    [SerializeField]
    public GameObject m_objToSpawn;
    [SerializeField]
    [Range(0, 100)]
    public float m_probability;
}

public class Chest : Interactable
{
    [SerializeField]
    private List<RandObj> m_contents;

    [SerializeField]
    private int m_minimumItemCount;

    [SerializeField]
    private int m_maxItemCount;

    [SerializeField]
    private float m_randMax;

    private float itemCount;
    private void Awake()
    {
        //insert random number of Items in chest
    
    }

    public override void Interact()
    {
        itemCount = Random.Range(m_minimumItemCount, m_maxItemCount + 1);
        for (int i = 0; i < itemCount; i++)
        {
            float randX = Random.Range(0.0f, m_randMax);
            float randY = Random.Range(0.0f, m_randMax);
            Vector3 temp = new Vector3(transform.position.x + randX, transform.position.y + randY, transform.position.z);

            int randIndex = GetRandomItemFromProbability();
         
            Instantiate(m_contents[randIndex].m_objToSpawn, temp, transform.rotation, this.transform.parent);
       
        }

        Destroy(gameObject);
    }

    int GetRandomItemFromProbability()
    {
        float total = 0;

        for (int i = 0; i < m_contents.Count; i++)
        {
            total += m_contents[i].m_probability;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < m_contents.Count; i++)
        {
            if (randomPoint < m_contents[i].m_probability)
            {
                return i;
            }
            else {
                randomPoint -= m_contents[i].m_probability;
            }
        }
        return m_contents.Count - 1;
    }
}
