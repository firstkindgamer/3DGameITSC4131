using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : EnemyTargeting
{
    public override AttackPriorityOptions type() {
        return AttackPriorityOptions.Tower;
    }

    // TOWER OPTIONS STUFF

    public void Upgrade()
    {

    }
    public void Salvage()
    {
        Destroy(gameObject);
    }
    public void Toggle()
    {
        
    }
    public void GroundAir()
    {

    }

    public string UpgradeText()
    {
        return "Upgrade";
    }
    public string SalvageText()
    {
        return "Salvage";
    }
    public string ToggleText()
    {
        return "Toggle";
    }
    public string GroundAirText()
    {
        return "Ground/Air";
    }

    //////////////////////

    public static Tower tower;
    private void Awake() 
    {
        tower = this;
    }

    [SerializeField]
    public TowerScriptableObject towerScriptableObject;
    public enum TowerTargetPriority{First, Last, Strongest, Weakest}
    public enum TowerTargetStyle{Ground, Air}


    protected List<GameObject> curEnemiesInRange = new List<GameObject>();
    protected GameObject curEnemy;
    private float fireCountdown;
    public TowerTargetPriority targetPriority;
    public TowerTargetStyle targetStyle;
    public Transform firePoint;
    public Transform partToRotate; 

    void Start()
    {
        towerScriptableObject.resetStats(); //resets stats from last session

        fireCountdown = towerScriptableObject.attackRate;
        SphereCollider myCollider = GetComponent<SphereCollider>();
        myCollider.radius = towerScriptableObject.range; //this does not update in real time
        
    }

    void Update()
    {
        if(curEnemy == null && curEnemiesInRange.Count != 0) //find an enemy if there is none
        {
            curEnemy = GetEnemy();
        }

        if(towerScriptableObject.rotateTowardsTarget && curEnemy != null) //rotate towards target
        {
            Vector3 dir = curEnemy.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            partToRotate.rotation = Quaternion.Euler (0f, rotation.y + towerScriptableObject.angleOffset, 0f); 
        }

        if (fireCountdown <= 0f)
        {
            curEnemy = GetEnemy();
            if(curEnemy != null)
            {
                Shoot();
                fireCountdown = towerScriptableObject.attackRate; //in case it was updated
            }  
        }
        fireCountdown -= Time.deltaTime; //count down till next Shoot()
    }

    GameObject GetEnemy()
    {
        curEnemiesInRange.RemoveAll(x => x == null);

        //runs to save resources if choice is obvious
        if(curEnemiesInRange.Count == 0)
            return null;
        if(curEnemiesInRange.Count == 1)
            return curEnemiesInRange[0];

        switch(targetPriority)
        {
            case TowerTargetPriority.First:
            {
                return curEnemiesInRange[0];
            }
            case TowerTargetPriority.Last:
            {
                int i = -1;
                foreach(GameObject enemyGO in curEnemiesInRange)
                {
                    i++;
                }
                return curEnemiesInRange[i];
            }
            case TowerTargetPriority.Strongest:
            {
                GameObject strongest = null;
                float strongestHealth = -1;
                
                foreach(GameObject enemyGO in curEnemiesInRange)
                {
                    Enemy enemy = enemyGO.GetComponent<Enemy>();
                    if(enemy.health > strongestHealth)
                    {
                        strongest = enemyGO;
                        strongestHealth = enemy.health;
                    }
                }
                return strongest;
            }
            case TowerTargetPriority.Weakest:
            {
                GameObject weakest = null;
                float weakestHealth = Mathf.Infinity;

                foreach(GameObject enemyGO in curEnemiesInRange)
                {
                    Enemy enemy = enemyGO.GetComponent<Enemy>();
                    if(enemy.health < weakestHealth)
                    {
                        weakest = enemyGO;
                        weakestHealth = enemy.health;
                    }
                }
                return weakest;
            }
        }
        return null;
    }

    public virtual void Shoot()
    {
        if(towerScriptableObject.rotateTowardsTarget) //rotate towards target
        {
            transform.LookAt(curEnemy.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        //Get the bullet ready to fire
        GameObject proj = Instantiate(towerScriptableObject.projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(curEnemy, towerScriptableObject.projectileDamage, towerScriptableObject.projectileSpeed, towerScriptableObject.bulletCleave, curEnemiesInRange, false);
        FindObjectOfType<AudioManager>().Play("bigShoot1");
    }


    private void OnTriggerEnter (Collider other) //Log enemies going in
    {
        if(targetStyle == TowerTargetStyle.Ground)
        {
            if(other.CompareTag("Ground"))
            {
                curEnemiesInRange.Add(other.gameObject);
            }
        }else if(targetStyle == TowerTargetStyle.Air)
        {
            if(other.CompareTag("Air"))
            {
                curEnemiesInRange.Add(other.gameObject);
            }
        }
        
    }

    private void OnTriggerExit (Collider other) //Log enemies going out
    {
        if(targetStyle == TowerTargetStyle.Ground)
        {
            if(other.CompareTag("Ground"))
            {
                curEnemiesInRange.Remove(other.gameObject);
            }
        }else if(targetStyle == TowerTargetStyle.Air)
        {
            if(other.CompareTag("Air"))
            {
                curEnemiesInRange.Remove(other.gameObject);
            }
        }
    }
}
