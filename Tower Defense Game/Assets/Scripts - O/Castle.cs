using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public GameObject castle;
    [SerializeField] public static int mainHealth;
    public static int healthLeft;
    public int damageTaken;

    void Update()
    {
        GetHealthPercentage();
    }
    
    void Start()
    {
        mainHealth = 500;
        healthLeft = mainHealth;
        
    }
    public float GetHealthPercentage()
    {
        return healthLeft / 500f; // Returns a value between 0 and 1
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        healthLeft = mainHealth -= damageTaken;
        Destroy(collision.gameObject);
    }
   
}
