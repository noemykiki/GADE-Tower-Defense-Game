using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject basicEnemy;
    private GameObject spawnTile;
    public GameObject healthBarPrefab;
    public float interval;
    public float timeBeforeStart;

    public bool roundStart;
    public bool roundGoing;
    // Start is called before the first frame update
    void Start()
    {
        roundStart = true;
        roundGoing = false;
  
    }
    private Coroutine spawnCoroutine; // Reference to the coroutine

    public void spawnEnemies()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(ISpawnEnemies());
    }

    public IEnumerator ISpawnEnemies()
    {
        for (int i = 0; i < 30; i++)
        {
            if (MapGenerator.startTile.Length == 0) yield break; // Ensure startTile is not empty

            int randomIndex = UnityEngine.Random.Range(0, MapGenerator.startTile.Length);
            GameObject spawnTile = MapGenerator.startTile[randomIndex];

            if (spawnTile != null) // Ensure spawnTile is not null
            {
                Instantiate(basicEnemy, spawnTile.transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Enemy.CleanUpDestroyedEnemies();
        if (roundStart) 
        {
            if(Time.time >= timeBeforeStart)
            {
                roundStart=false;
                roundGoing=true;
                spawnEnemies();
            }
        }
    }
}
