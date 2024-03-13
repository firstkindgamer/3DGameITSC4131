using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public Transform target;
    public float range = 15f;
    public string enemyTag = "Enemy";
    public Transform partToRotate; 

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //Update target every half second
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) //if no target do nothing
            return;
        // Vector3 dir = target.position = transform.position;
        // Quaternion lookRotation = Quaternion.LookRotation(dir);
        // Vector3 rotation = lookRotation.eulerAngles;
        // partToRotate.rotation = Quaternion.Euler (0f, rotation.y, 0f);
    }

    void OnDrawGizmosSelected() //Draws range if tower is selected in unity
    {
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
