using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject basicEnemy;
    private GameObject spawnTile;
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
    
    private void spawnEnemies()
    {
        StartCoroutine("ISpawnEnemies");
    }

    IEnumerator ISpawnEnemies()
    {
        for (int i = 0; i < 30;i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, MapGenerator.startTile.Length);
            spawnTile = MapGenerator.startTile[randomIndex];
            GameObject newEnemy = Instantiate(basicEnemy,spawnTile.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }
    // Update is called once per frame
    void Update()
    {
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
