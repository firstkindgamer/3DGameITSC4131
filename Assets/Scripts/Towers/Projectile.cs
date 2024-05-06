using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mono.Cecil;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.InputSystem;
using static FreezeTower;

public class Projectile : MonoBehaviour
{
    public static Projectile projectile;
    void Awake() => projectile = this;
    private GameObject target;
    private float damage;
    private float moveSpeed;
    private bool isCleave;
    public int cleaveNumber = 2;
    private bool isFreeze;
    private List<GameObject> cleaveTargets = new List<GameObject>();
    

    public GameObject hitSpawnPrefab;

    
    public void Initialize(GameObject target, float damage, float moveSpeed, bool cleave, List<GameObject> game, bool freeze)
    {
        this.target = target;
        this.damage = damage;
        this.moveSpeed = moveSpeed;
        this.isCleave = cleave;
        this.isFreeze = freeze;

        for(int i = 0; i < game.Count; i++) //initilizing it like the others links the lists somehow????
        {
            cleaveTargets.Add(game[i]);
        }
    }

    void Update()
    {
        if(target != null && !isCleave)
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
        }else if(target != null && isCleave)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
            transform.LookAt(target.transform);
            if(Vector3.Distance(transform.position, target.transform.position) < 0.2f)
            {
                TakeDamage(target, damage);
                cleaveTargets.Remove(target);
                try{
                    if(isFreeze)  
                    {
                        StartCoroutine(fTower.Freeze());
                    }
                }catch(Exception ex2)
                {
                    Debug.Log("Enemy Movement is not properly configured!");
                }
                
                if(hitSpawnPrefab != null)
                {
                    GameObject effectIns = (GameObject)Instantiate(hitSpawnPrefab, transform.position, Quaternion.identity);
                    Destroy(effectIns, 2f);
                }

                if(cleaveNumber != 0)
                {
                    if(cleaveTargets.Count > 0)
                    {
                        target = findClosestEnemy();
                        moveSpeed *= 1.5f;
                        cleaveNumber--;
                    } else Destroy(gameObject);
                } else Destroy(gameObject);
                
            }
        }else
        {
            Destroy(gameObject);
        }
    
    }

    void TakeDamage(GameObject target, float damage)
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

    GameObject findClosestEnemy()
    {
        GameObject closest = null;
            float dist = Mathf.Infinity;

            for(int x = 0; x < cleaveTargets.Count; x++)
                {
                    float d = (transform.position - cleaveTargets[x].transform.position).sqrMagnitude;

                    if(d < dist)
                    {
                        closest = cleaveTargets[x];
                        dist = d;
                    }
                }
            return closest;
    }

}
