using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    public Transform pivot;
    public Enemy enemy;

    private void Update()
    {
        if (enemy != null)
        {
            if (enemy.currentTarget != null)
            {
                Vector2 relative = enemy.currentTarget.transform.position - pivot.position;
                float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;
                Vector3 newRotation = new Vector3(0, 0, angle);
                pivot.localRotation = Quaternion.Euler(newRotation);
            }
        }
    }
}
