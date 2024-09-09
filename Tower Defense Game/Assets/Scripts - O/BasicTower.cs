using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Tower
{
    public GameObject bullet;
    public Transform pivot;
    public Transform canon;

    protected override void shoot()
    {
        base.shoot();

        GameObject newBullet = Instantiate(bullet,canon.position,pivot.rotation);
    }
}
