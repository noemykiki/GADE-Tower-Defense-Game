using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Transform pivot;
    public Tower tower;

    private void Update()
    {
        if (tower != null)
        {
            if (tower.currentTarget != null)
            {
                Vector2 relative = tower.currentTarget.transform.position - pivot.position;
                float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            }
        }
    }
}
