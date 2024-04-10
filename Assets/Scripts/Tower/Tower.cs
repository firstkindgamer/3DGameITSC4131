using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static SkillTree;
using static Skill;

public class Tower : MonoBehaviour
{
    [Header("Attributes")]
    public Transform target;
    public float range = 15f;
    public float fireRate = 1f; //fire once a second by default
    private float fireCountdown = 0f; //is divided by firerate

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate; 

    public GameObject bulletPrefab;
    public Transform firePoint;
    
    float fireRateModifier;
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //Update target every half second
    }

    void UpdateTarget()
    {
        //Update values based on skill tree
        //fireRateModifier = SkillTree.SkillLevels[0];
        //fireRate = 1 - SkillTree.SkillLevels[0];


        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies) //find the nearest enemy
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            //print("Switching target!");
            target = nearestEnemy.transform; //switch target to nearest enemy
        } else {
            target = null;
            //print("There are no enemies to target!");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) //if no target do nothing
            return;
        
        //Rotates partToRotate to face target
        //BUG: Some part of this teleports new objects into the base of the tower. 
        //Tagged until we have a better way to debug

         Vector3 dir = target.position - transform.position;
         Quaternion lookRotation = Quaternion.LookRotation(dir);
         Vector3 rotation = lookRotation.eulerAngles;
         partToRotate.rotation = Quaternion.Euler (0f, rotation.y + 90f, 0f); //-180 is an offset for clockwork tower

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime; 
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected() //Draws range if tower is selected in unity
    {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
