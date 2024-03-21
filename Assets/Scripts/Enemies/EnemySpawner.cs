using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyBehaviorScriptableObject walkingEnemyBehaviors;
    public EnemyBehaviorScriptableObject flyingEnemyBehaviors;

    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnFlying());

        StartCoroutine(spawnWalking());
    }

    IEnumerator spawnFlying()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f + Random.value * 10);
            Enemy newEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
            newEnemy.enemyBehaviors = flyingEnemyBehaviors;
            newEnemy.transform.position = transform.position;
            newEnemy.Init();
        }
    }

    IEnumerator spawnWalking()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f + Random.value * 5);
            Enemy newEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
            newEnemy.enemyBehaviors = walkingEnemyBehaviors;
            newEnemy.transform.position = transform.position;
            newEnemy.Init();
        }
    }
}
