using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static Tower tower;
    private void Awake() => tower = this;

    [SerializeField]
    private TowerScriptableObject towerScriptableObject;
    public enum TowerTargetPriority{First, Last, Strongest, Weakest}
    public enum TowerType{Basic, Gattling, Crossbow}

    [Header("Attributes")]
    public float range;
    private List<GameObject> curEnemiesInRange = new List<GameObject>();
    private GameObject curEnemy;
    public float attackRate;
    private float fireCountdown;
    public TowerTargetPriority targetPriority;
    
    [Header("Bullet Settings")]
    public GameObject projectilePrefab;
    public int projectileDamage;
    public float projectileSpeed;
    public bool bulletCleave;

    [Header("Set Up")]
    public Transform firePoint;
    public Transform partToRotate; 
    public float angleOffset;
    public bool rotateTowardsTarget;

    

    

    void Start()
    {
        fireCountdown = attackRate;
        /*
        fireCountdown = towerScriptableObject.attackRate;
        range = towerScriptableObject.range;
        attackRate = towerScriptableObject.attackRate;
        projectileDamage = towerScriptableObject.projectileDamage;
        projectileSpeed = towerScriptableObject.projectileSpeed;
        bulletCleave = towerScriptableObject.bulletCleave;
        angleOffset = towerScriptableObject.angleOffset;
        rotateTowardsTarget = towerScriptableObject.rotateTowardsTarget;
        */

        SphereCollider myCollider = GetComponent<SphereCollider>();
        myCollider.radius = range;
    }

    void Update()
    {
        if(curEnemy == null && curEnemiesInRange.Count != 0)
        {
            curEnemy = GetEnemy();
        }

        if(rotateTowardsTarget && curEnemy != null) //Note this only rotates tower when shooting, not in idle
        {
            Vector3 dir = curEnemy.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            partToRotate.rotation = Quaternion.Euler (0f, rotation.y + angleOffset, 0f); 
        }

        if (fireCountdown <= 0f)
        {
            curEnemy = GetEnemy();
            if(curEnemy != null)
            {
                Shoot();
                fireCountdown = attackRate;
            }  
        }
        fireCountdown -= Time.deltaTime; 
    }

    GameObject GetEnemy()
    {
        curEnemiesInRange.RemoveAll(x => x == null);

        //runs to save resources if choise is obvious
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

    void Shoot()
    {
        if(rotateTowardsTarget)
        {
            transform.LookAt(curEnemy.transform);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<Projectile>().Initialize(curEnemy, projectileDamage, projectileSpeed, bulletCleave, curEnemiesInRange);
        FindObjectOfType<AudioManager>().Play("bigShoot1");
    }


    private void OnTriggerEnter (Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            print("Entering!");
            curEnemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            curEnemiesInRange.Remove(other.gameObject);
        }
    }
}
