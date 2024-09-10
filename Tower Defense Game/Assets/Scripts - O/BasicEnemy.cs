using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public GameObject bullet;
    public Transform pivot;
    public Transform slime;

    protected override void shoot()
    {
        base.shoot();

        GameObject newBullet = Instantiate(bullet, slime.position, pivot.rotation);
    }
}
