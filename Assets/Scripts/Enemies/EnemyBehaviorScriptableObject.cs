using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[CreateAssetMenu(fileName = "EnemyBehavior", menuName = "ScriptableObjects/EnemyBehaviorScriptableObject", order = 1)]
public class EnemyBehaviorScriptableObject : ScriptableObject
{
    public GameObject visibleObjectPrefab;

    public bool stopsForTowers;

    public List<AttackPriorityOptions> attackPriority;

    //represented in graph as walkable height
    //represented in follower entity as layer & collides with
    //public bool isFlying;

    //represented in graph as radius
    //public float radius;

    public GraphMask traversableGraphs;
    public int traversableGraphIndex = 0;

    public float speed = 1f;

    public float attackRate = 5f;

    [Header("Set to 0 for melee enemies.")]
    public float range = 0f;

    [Header("Affects spawned bullets only.")]
    public float bulletSpeed = 0.05f;
    public float bulletSize = 1f;
    public float damage = 1f;
}

public enum AttackPriorityOptions
{
    Player,
    Tower,
    Base
}