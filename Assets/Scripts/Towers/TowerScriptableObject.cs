using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    [Header("Set Up Values")]
    public float baseRange = 35;
    public float baseAttackRate = 1;
    public float baseProjectileDamage = 20;
    public float baseProjectileSpeed = 50;
    public bool baseBulletCleave = false;
    public GameObject projectilePrefab;
    public float angleOffset = 100;
    public int price;
    public bool rotateTowardsTarget = true;
    public float maxHealth;
    
    [Header("Changed During Gameplay")]
    public float range = 35;
    public float attackRate = 1;
    public float projectileDamage = 20;
    public float projectileSpeed = 50;
    public bool bulletCleave = false;
    public float health;

    public void resetStats()
    {
        range = baseRange;
        attackRate = baseAttackRate;
        projectileDamage = baseProjectileDamage;
        projectileSpeed = baseProjectileSpeed;
        bulletCleave = baseBulletCleave;
        health = maxHealth;
    }
    
}
