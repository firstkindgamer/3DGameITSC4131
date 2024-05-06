using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector3 direction;
    public Vector3 startingPoint;
    public EnemyBehaviorScriptableObject enemyBehaviors; //use shooting distance from enemy, NOT range from enemyBehaviors
    public float maxTravelDistance;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(enemyBehaviors.bulletSize, enemyBehaviors.bulletSize, enemyBehaviors.bulletSize);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target.position, enemyBehaviors.bulletSpeed);
        transform.position += direction * enemyBehaviors.bulletSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, startingPoint) > maxTravelDistance)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyTargeting")
        {
            EnemyTargeting hit = other.transform.parent.GetComponent<EnemyTargeting>();
            hit.changeHealth(-1);
        }
    }
}
