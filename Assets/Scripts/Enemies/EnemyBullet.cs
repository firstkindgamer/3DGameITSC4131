using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Transform target;
    public EnemyBehaviorScriptableObject enemyBehaviors;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(enemyBehaviors.bulletSize, enemyBehaviors.bulletSize, enemyBehaviors.bulletSize);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, enemyBehaviors.bulletSpeed);
    }
}
