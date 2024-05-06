using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;
using System;

public class FreezeTower : Tower
{
    private Color ogColor;
    public static FreezeTower fTower;
    public void Awake()
    {
        fTower = this;
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
        proj.GetComponent<Projectile>().Initialize(curEnemy, towerScriptableObject.projectileDamage, towerScriptableObject.projectileSpeed, towerScriptableObject.bulletCleave, curEnemiesInRange, true);
        FindObjectOfType<AudioManager>().Play("FreezeTower"); //changed audio from Tower
    }

    public IEnumerator Freeze()
    {
        //TODO: Fix not changing Color
        //ogColor = curEnemy.GetComponent<Renderer>().material.GetColor("_ogColor");
        float baseSpeed = enemy.followerEntity.maxSpeed;
        if(enemy.followerEntity.maxSpeed >= enemy.followerEntity.maxSpeed * .5) //prevent debuff from applying more than once
        {
            enemy.followerEntity.maxSpeed *= .5f; 
            //curEnemy.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); //give the enemy a frozen apperance 
        }
        yield return new WaitForSeconds(5); //wait 5 seconds
        enemy.followerEntity.maxSpeed = baseSpeed;
        //curEnemy.GetComponent<Renderer>().material.SetColor("_ogColor", ogColor);}}
    }

}
