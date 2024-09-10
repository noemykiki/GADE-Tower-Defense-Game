using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public GameObject castle;
    [SerializeField] private int mainHealth;
    public static int healthLeft;

    void Start()
    {
        healthLeft = mainHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        healthLeft = mainHealth -= 5;
        Destroy(collision.gameObject);
    }
}
