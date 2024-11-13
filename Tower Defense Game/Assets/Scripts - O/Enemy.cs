using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Assuming you need this for the Slider component
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    private float maxHealth;
    [SerializeField] private float enemySpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private double reward;
    private DamageEffectController damageFlash;


    public HealthBar healthBarPrefab; // Reference to the HealthBar prefab
    private HealthBar healthBar;
    private float nextTimeShoot;
    public GameObject targetTile;
    public GameObject currentTarget;
    public static double totalReward = 100;
    private GameObject healthBarObject;
    private void Awake()
    {
        Enemies.enemies.Add(gameObject);
    }

    void Start()
    {
        damageFlash = GetComponent<DamageEffectController>();
        maxHealth = enemyHealth;
        if (healthBarPrefab != null)
        {
            // Instantiate the HealthBar prefab and set it up in the world
            healthBarObject = Instantiate(healthBarPrefab.gameObject);
            healthBar = healthBarObject.GetComponent<HealthBar>();
            healthBar.maxHealth = enemyHealth;
            healthBar.SetHealth(enemyHealth);

            // Set the health bar's parent to this enemy, if desired
            healthBarObject.transform.SetParent(transform);
            healthBarObject.transform.localPosition = new Vector3(0, 1.5f, 0); // Position it above the enemy
        }
        InitializeEnemy();
    }

    void Update()
    {
        if (healthBar != null)
        {
            // Update the health bar's position above the enemy in world space
            healthBarObject.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }
        SpriteRenderer fillRenderer = healthBar.fillRenderer;
        float healthPercentage = enemyHealth / maxHealth;

        // Adjust the scale of the fill sprite based on health percentage
        fillRenderer.transform.localScale = new Vector3(healthPercentage, 1, 1);

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
        CleanUpDestroyedEnemies();
    }

    private void InitializeEnemy()
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
        enemyHealth -= -amount;
        if (damageFlash != null)
        {
            damageFlash.TriggerDamageEffects();
        }
        if (healthBar != null)
        {
            healthBar.SetHealth(enemyHealth);
        }
        if (enemyHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {
        Enemies.enemies.Remove(gameObject);
        if (EnemySpawn.isBonusFrenzyActive)
        {
            reward *= 1.5;
            totalReward += reward;
        }
        else
        {
            totalReward += reward;
        }
        

        totalReward += reward;
        if (healthBarObject != null)
        {
            Destroy(healthBarObject); // Destroy the health bar object when the enemy dies
        }
        Destroy(gameObject);
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

    public static void CleanUpDestroyedEnemies()
    {
        Enemies.enemies.RemoveAll(item => item == null);
    }
}
