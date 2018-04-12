using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    //list of enemies to choose from
    [SerializeField]
    [Tooltip("Enemies to choose from.")]
    List<EnemySpawnData> _enemies = new List<EnemySpawnData>();

    void Start()
    {
        //make sure we have atleast 1 enemy to spawn.
        if (_enemies.Count == 0)
        {
            Debug.LogWarning("No enemies assigned to spawn on spawner: " + name);
            return;
        }
        //choose an enemy to spawn
        ChooseEnemyToSpawn();

        //Destroy the spawner
        Destroy(gameObject);
    }

    //choose which enemy we want to spawn dependant on their probability.
    private void ChooseEnemyToSpawn()
    {
        //total chance to spawn enemies
        int totalChance = 0;
        for (int i = 0; i < _enemies.Count; ++i)
        {
            //sum up all the chances.
            totalChance += _enemies[i].chance;
        }
        //make sure the total chance is more than 0
        if (totalChance <= 0)
        {
            Debug.LogWarning("Total chance of spawning an enemy has to be more than 0 on spawner: " + name);
            return;
        }
        int randomChance = Random.Range(1, totalChance);
        //index of the enemy that we have chosen to spawn
        int chosenIndex = 0;
        for (int i = 0; i < _enemies.Count && randomChance > 0; ++i)
        {
            randomChance -= _enemies[i].chance;
            if (randomChance <= 0)
            {
                chosenIndex = i;
                break;
            }
        }


        //spawn an enemy of an index that we have decided on.
        SpawnEnemy(chosenIndex);
    }

    //spawn an enemy under the given index.
    private void SpawnEnemy(int index)
    {
        //spawn an enemy.
        Instantiate( _enemies[index].enemy, transform.position, Quaternion.identity, transform.parent);
    }
}

[System.Serializable]
public class EnemySpawnData
{
    [SerializeField]
    GameObject _enemy = null;

    public GameObject enemy{get{ return _enemy; } }

    [SerializeField]
    int _chance = 0;
    public int chance { get{ return _chance; } }
}
