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
        //StartCoroutine(spawnFlying());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("The Swarm is Coming");
            StartCoroutine(spawnFlying());
        }
    }



    IEnumerator spawnFlying()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f + Random.value * 4);
            int index = UnityEngine.Random.Range(0, enemyBehaviors.Length - 1);
            Enemy newEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
            newEnemy.enemyBehaviors = enemyBehaviors[index];
            newEnemy.transform.position = transform.position;
            newEnemy.Init();
        }
    }
}
