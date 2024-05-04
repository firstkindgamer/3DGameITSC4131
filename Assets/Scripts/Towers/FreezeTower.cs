using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        proj.GetComponent<Projectile>().Initialize(curEnemy, towerScriptableObject.projectileDamage, towerScriptableObject.projectileSpeed, towerScriptableObject.bulletCleave, curEnemiesInRange);
        FindObjectOfType<AudioManager>().Play("FreezeTower"); //changed audio from Tower
    }
/*
    public void Freeze()
    {
        ogColor = curEnemy.GetComponent<Renderer>().material.GetColor("_ogColor");
        float baseSpeed = curEnemy.moveSpeed;
        if(curEnemy.moveSpeed >= curEnemy.moveSpeed * .5) //prevent debuff from applying more than once
        {
            curEnemy.moveSpeed *= .5; 
            curEnemy.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); //give the enemy a frozen apperance 
        }

        yield return new WaitForSeconds(5); //wait 5 seconds
        curEnemy.moveSpeed = baseSpeed;
        curEnemy.GetComponent<Renderer>().material.SetColor("_ogColor", ogColor);
        
    }
*/
}
