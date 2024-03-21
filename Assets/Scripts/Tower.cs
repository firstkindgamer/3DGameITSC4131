using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public Transform target;
    public float range = 15f;
    public Transform partToRotate;

    public Quaternion rotateDir = Quaternion.identity;

    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Fire", 0f, 1.5f); //Update target every half second
    }

    void Update()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float shortestDistance = Mathf.Infinity;
        Enemy nearestEnemy = null;
        foreach (Enemy enemy in enemies)
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
            target = nearestEnemy.transform;
        } else
        target = null;

        var lookDir = nearestEnemy.transform.position - transform.position;
        lookDir.y = 0; // keep only the horizontal direction
        rotateDir = Quaternion.LookRotation(lookDir);

        partToRotate.rotation = Quaternion.RotateTowards(partToRotate.rotation, rotateDir, 10f);
    }

    void Fire()
    {
        if (target != null)
        {
            Bullet newBullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            newBullet.transform.position = transform.position + new Vector3(0,1,0);
            newBullet.goalDir = target.transform.position;

        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (target == null) //if no target do nothing
    //        return;
    //    // Vector3 dir = target.position = transform.position;
    //    // Quaternion lookRotation = Quaternion.LookRotation(dir);
    //    // Vector3 rotation = lookRotation.eulerAngles;
    //    // partToRotate.rotation = Quaternion.Euler (0f, rotation.y, 0f);
    //}

    void OnDrawGizmosSelected() //Draws range if tower is selected in unity
    {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
