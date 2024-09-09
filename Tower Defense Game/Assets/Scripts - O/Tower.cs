using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tower : MonoBehaviour
{

    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    private float nextTimeShoot;

    public  GameObject currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        nextTimeShoot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        nearestEnemy();
        if (Time.time > nextTimeShoot)
        {
            if (currentTarget != null)
            {
                shoot();
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
}
