using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
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
                float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
                Vector3 newRotation = new Vector3(0,0, angle);
                pivot.localRotation = Quaternion.Euler(newRotation);
            }
        }
    }
}
