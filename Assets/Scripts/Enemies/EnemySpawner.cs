using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyBehaviorScriptableObject[] enemyBehaviors;

    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnFlying());
    }

    IEnumerator spawnFlying()
    {
        int index = 0;
        while (true)
        {
            yield return new WaitForSeconds(3f + Random.value * 4);
            Enemy newEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
            newEnemy.enemyBehaviors = enemyBehaviors[index];
            newEnemy.transform.position = transform.position;
            newEnemy.Init();
            index = (index + 1) % enemyBehaviors.Length;
        }
    }
}
