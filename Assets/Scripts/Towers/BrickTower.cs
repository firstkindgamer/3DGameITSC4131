using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickTower : Tower
{
    public static BrickTower bTower;
    public void Awake()
    {
        bTower = this;
    }

    public override void Shoot()
    {
        if(towerScriptableObject.rotateTowardsTarget) //rotate towards target
        {
            transform.LookAt(curEnemy.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        //Get the bullet ready to fire
        GameObject proj = Instantiate(towerScriptableObject.projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(curEnemy, towerScriptableObject.projectileDamage, towerScriptableObject.projectileSpeed, towerScriptableObject.bulletCleave, curEnemiesInRange, false);
        FindObjectOfType<AudioManager>().Play("BrickTower"); //changed audio from Tower
    }
}
