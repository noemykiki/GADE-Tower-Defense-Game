using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] public float towerHealth;
    private float maxHealth;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;

    public HealthBar healthBarPrefab; // Reference to the HealthBar prefab
    private HealthBar healthBar;
    private int damageTaken;
    private float nextTimeShoot;
    
    private GameObject healthBarObject;
    public  GameObject currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = towerHealth;
        if (healthBarPrefab != null)
        {
            // Instantiate the HealthBar prefab and set it up in the world
            healthBarObject = Instantiate(healthBarPrefab.gameObject);
            healthBar = healthBarObject.GetComponent<HealthBar>();
            healthBar.maxHealth = towerHealth;
            healthBar.SetHealth(towerHealth);

            // Set the health bar's parent to this enemy, if desired
            healthBarObject.transform.SetParent(transform);
            healthBarObject.transform.localPosition = new Vector3(0, 0.5f, 0);
            nextTimeShoot = Time.time;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar != null)
        {
            // Update the health bar's position above the enemy in world space
            healthBarObject.transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }
        SpriteRenderer fillRenderer = healthBar.fillRenderer;
        float healthPercentage = towerHealth / maxHealth;

        // Adjust the scale of the fill sprite based on health percentage
        fillRenderer.transform.localScale = new Vector3(healthPercentage, 1, 1);
        nearestEnemy();

        if (Time.time > nextTimeShoot)
        {
            if (currentTarget != null)
            {
                shoot();
                nextTimeShoot = Time.time + fireRate;
            }

        }
        
    }

    private void nearestEnemy()
    {
        GameObject nearestEnemy = null;

        float distance = Mathf.Infinity;
  
        foreach(GameObject enemy in Enemies.enemies)
        {
            if (enemy != null)
            {
                float _distance = (transform.position - enemy.transform.position).magnitude;
                if (_distance < distance)
                {
                    distance = _distance;
                    nearestEnemy = enemy;
                }
            }
        }

        if (distance <= range)
        {
            currentTarget = nearestEnemy;
        }
        else
        {
            currentTarget = null;
        }
    }
    protected virtual void shoot()
    {
      
          Enemy enemyScript = currentTarget.GetComponent<Enemy> ();

          enemyScript.takeDamage(-damage);
        
    }
    public void takeDamage(float amount)
    {
        towerHealth += amount;

        if (towerHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {

        Towers.towers.Remove(gameObject);
        Destroy(transform.gameObject);
    }
}
