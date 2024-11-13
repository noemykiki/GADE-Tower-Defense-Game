using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public GameObject castle;
    [SerializeField] public static int mainHealth;
    public static int healthLeft;
    public int damageTaken;
    private DamageEffectController damageFlash;

    void Update()
    {
        GetHealthPercentage();
    }
    
    void Start()
    {
        damageFlash = GetComponent<DamageEffectController>();
        mainHealth = 500;
        healthLeft = mainHealth;
        
    }
    public float GetHealthPercentage()
    {
        return healthLeft / 500f; // Returns a value between 0 and 1
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (damageFlash != null)
        {
            damageFlash.TriggerDamageEffects();
        }
        healthLeft = mainHealth -= damageTaken;
        Destroy(collision.gameObject);
    }
   
}
