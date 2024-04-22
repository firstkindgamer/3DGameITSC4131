using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Skill;
using static SkillTree;

public class Tower : MonoBehaviour
{
    [Header("Attributes")]
    public Transform target;
    public float range = 15f;
    public float baseFireRate = 1f; //fire once a second by default
    private float fireCountdown = 0f; //is divided by firerate
    public enum Mode {First, Close, Distant}
    public Mode fireMode;
    public enum Targeting {Ground, Air, GroundAir}
    public Targeting targetMethod;
    public float angleOffset = 90;

    [Header("Unity Setup Fields")]
    public string groundTag = "Enemy";
    public string airTag = "AirEnemy";
    public Transform partToRotate; 

    public GameObject bulletPrefab;
    public Transform firePoint;
    List<GameObject> enemiesInRange = new List<GameObject>();
    
    float fireRateModifier;
    

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, baseFireRate/2); //Update target every half second
    }

    void UpdateTarget()
    {
        //Update values based on skill tree
        fireRateModifier = skillTree.SkillLevels[0];
        float fireRate;
        fireRate = baseFireRate - skillTree.SkillLevels[0];


        GameObject[] enemies = GameObject.FindGameObjectsWithTag(groundTag); //default ground to not bug out other code
        if (targetMethod == Targeting.GroundAir)
        {
            //No clue head empty FIX THIS
        }
        if (targetMethod == Targeting.Ground)
        {
            //Continue
        } else if(targetMethod == Targeting.Air)
        {
            enemies = GameObject.FindGameObjectsWithTag(airTag);
        }

        float shortestDistance = Mathf.Infinity;
        float farthestDistance = 0;
        GameObject closestEnemy = null;
        GameObject farthestEnemy = null;

        foreach (GameObject enemy in enemies) //find the enemies
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (fireMode == Mode.First)
            {
                if(distanceToEnemy <= range)
                {
                    if (!enemiesInRange.Contains(enemy))
                    {
                        enemiesInRange.Add(enemy);
                    }
                }
            }else if (fireMode == Mode.Distant)
            {
                if (distanceToEnemy > farthestDistance && distanceToEnemy <= range)
                {
                    farthestDistance = distanceToEnemy;
                    farthestEnemy = enemy;
                }
            } else if(fireMode == Mode.Close)
            {
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = enemy;
                }
            }
            

        }

        if (fireMode == Mode.First)
        {
            if(enemiesInRange[0] != null)
            {
                target = enemiesInRange.Find(x => x).transform; //gets the first enemy in range
                //enemiesInRange.Clear();
            } else{
                target = null;
                //enemiesInRange.Clear();
            }
        }else if(fireMode == Mode.Close)
        {
            if (closestEnemy != null && shortestDistance <= range)
            {
                target = closestEnemy.transform; //switch target to nearest enemy
            } else {
                target = null;
            }
        }else if(fireMode == Mode.Distant)
        {
            if (farthestEnemy != null && farthestDistance <= range)
            {
                //print("Switching target!");
                target = farthestEnemy.transform; //switch target to nearest enemy
            } else {
                target = null;
                //print("There are no enemies to target!");
            }
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
         partToRotate.rotation = Quaternion.Euler (0f, rotation.y + angleOffset, 0f); //-180 is an offset for clockwork tower

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / baseFireRate - skillTree.SkillLevels[0];
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
