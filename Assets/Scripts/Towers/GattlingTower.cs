using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GattlingTower : Tower
{
    public static GattlingTower gTower;
    public void Awake()
    {
        gTower = this;
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
        FindObjectOfType<AudioManager>().Play("GattlingTower"); //changed audio from Tower
    }
}
