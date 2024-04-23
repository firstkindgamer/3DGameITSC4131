using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProjectile : MonoBehaviour
{
    private GameObject target;
    private int damage;
    private float moveSpeed;

    public GameObject hitSpawnPrefab;

    public void Initialize(GameObject target, int damage, float moveSpeed)
    {
        this.target = target;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
    }

    void Update()
    {
        if(target != null)
        {

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            transform.LookAt(target.transform);
            if(Vector3.Distance(transform.position, target.transform.position) < 0.2f)
            {
                TakeDamage(target, damage);
                
                if(hitSpawnPrefab != null)
                {
                    GameObject effectIns = (GameObject)Instantiate(hitSpawnPrefab, transform.position, Quaternion.identity);
                    Destroy(effectIns, 2f);
                }
                Destroy(gameObject);
            }
        }else
        {
            Destroy(gameObject);
        }
    
    }

    void TakeDamage(GameObject target, int damage)
    {
        try
        {
            target.GetComponent<Enemy>().health -= damage;
            if(target.GetComponent<Enemy>().health <= 0)
            {
                Destroy(target);
            }
        }catch (NullReferenceException ex1)
        {
            //This was clogging up Console
            Debug.Log("The targets health is not properly configured!");
        }
    }

}
