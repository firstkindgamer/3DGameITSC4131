using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NewTower : MonoBehaviour
{
    public enum TowerTargetPriority{First, Close, Strong}

    [Header("Attributes")]
    public float range;
    private List<GameObject> curEnemiesInRange = new List<GameObject>();
    private GameObject curEnemy;

    public TowerTargetPriority targetPriority;
    public bool rotateTowardsTarget;

    [Header("Set Up")]
    public float attackRate;
    private float fireCountdown;
    public GameObject projectilePrefab;
    public Transform firePoint;


    public int projectileDamage;
    public float projectileSpeed;

    public Transform partToRotate; 
    public float angleOffset;

    void Start()
    {
        fireCountdown = attackRate;
        //This could cause issues if you update firerate during gameplay
        //Oh well!

        SphereCollider myCollider = GetComponent<SphereCollider>();
        myCollider.radius = range;
        //same thing LOL
    }

    void Update()
    {
         
        if (fireCountdown <= 0f)
        {
            curEnemy = GetEnemy();
            if(curEnemy != null)
            {
                if(rotateTowardsTarget)
                {
                    Vector3 dir = curEnemy.transform.position - transform.position;
                    Quaternion lookRotation = Quaternion.LookRotation(dir);
                    Vector3 rotation = lookRotation.eulerAngles;
                    partToRotate.rotation = Quaternion.Euler (0f, rotation.y + angleOffset, 0f); 
                }
                
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
            case TowerTargetPriority.Close:
            {
                GameObject closest = null;
                float dist = 99;

                for(int x = 0; x < curEnemiesInRange.Count; x++)
                {
                    float d = (transform.position - curEnemiesInRange[x].transform.position).sqrMagnitude;

                    if(d < dist)
                    {
                        closest = curEnemiesInRange[x];
                        dist = d;
                    }
                }
                return closest;
            }
            case TowerTargetPriority.Strong:
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
        proj.GetComponent<NewProjectile>().Initialize(curEnemy, projectileDamage, projectileSpeed);
    }


    private void OnTriggerEnter (Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
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
