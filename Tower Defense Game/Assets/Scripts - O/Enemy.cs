using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
  [SerializeField]  private float enemySpeed;
    private int reward;
    private int damageTaken;
    public GameObject targetTile;

    private void Awake()
    {
        Enemies.enemies.Add(gameObject);
    }

    private void Start()
    {
        inisitaliseEnemy();
    }

    private void Update()
    {
        checkPosition();
        enemyMovement();
    }

    private void inisitaliseEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, MapGenerator.startTile.Length);
        targetTile = MapGenerator.startTile[randomIndex];
    }

    private void enemyMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, enemySpeed * Time.deltaTime);
    }

    public void takeDamage(float amount)
    {
        enemyHealth += amount;

        if (enemyHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {

        Enemies.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    private void checkPosition()
    {
        if (targetTile != null && targetTile != MapGenerator.endTile)
        {
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if (distance < 0.001f)
            {
                int currentIndex = MapGenerator.pathTiles.IndexOf(targetTile);
                targetTile = MapGenerator.pathTiles[currentIndex + 1];
            }
        }
    }
}