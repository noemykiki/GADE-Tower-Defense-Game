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
    [SerializeField] private float fireRate;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    private int reward;
    private int damageTaken;
    private float nextTimeShoot;
    public GameObject targetTile;
    public GameObject currentTarget;


    private void Awake()
    {
        Enemies.enemies.Add(gameObject);
    }
void Start()
    {
        nextTimeShoot = Time.time;
        inisitaliseEnemy();
    }

   void Update()
    {
        nearestTower();

        if (Time.time > nextTimeShoot)
        {
            if (currentTarget != null)
            {
                shoot();
                nextTimeShoot = Time.time + fireRate;
            }

        }
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

    private void nearestTower()
    {
        GameObject nearestTower = null;

        float distance = Mathf.Infinity;

        foreach (GameObject tower in Towers.towers)
        {
            if (tower != null)
            {
                float _distance = (transform.position - tower.transform.position).magnitude;
                if (_distance < distance)
                {
                    distance = _distance;
                    nearestTower = tower;
                }
            }
        }

        if (distance <= range)
        {
            currentTarget = nearestTower;
        }
        else
        {
            currentTarget = null;
        }
    }
    protected virtual void shoot()
    {

        Tower towerScript = currentTarget.GetComponent<Tower>();

        towerScript.takeDamage(-damage);

    }
}