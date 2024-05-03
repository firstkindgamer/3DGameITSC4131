using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    [Header("Attributes")]
    public float range = 35;
    public float attackRate = 1;

    [Header("Bullet Settings")]
    public GameObject projectilePrefab;
    public int projectileDamage = 20;
    public float projectileSpeed = 50;
    public bool bulletCleave = false;

    [Header("Set Up")]
    public float angleOffset = 100;
    public bool rotateTowardsTarget = true;
}
