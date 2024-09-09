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
        Vector3 spawnPosition = new Vector3(canon.position.x, canon.position.y, -1);
        GameObject newBullet = Instantiate(bullet,spawnPosition,pivot.rotation);
    }
}
