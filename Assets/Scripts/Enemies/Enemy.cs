using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    private Seeker seeker;
    public AIDestinationSetter goalDest;
    private FollowerEntity followerEntity;
    public RecastGraph myRecastGraph;

    public bool isFlying;
    public float radius;
    public bool isRanged;

    public float health = 10;

    public EnemyBehaviorScriptableObject enemyBehaviors;

    // Start is called before the first frame update
    public void Init()
    {
        AstarPath path = FindFirstObjectByType<AstarPath>();
        myRecastGraph = getMyRecastGraph(path);

        followerEntity = GetComponent<FollowerEntity>();
        seeker = GetComponent<Seeker>();
        goalDest = GetComponent<AIDestinationSetter>();

        isFlying = checkIfFlying();
        radius = myRecastGraph.characterRadius;
        isRanged = enemyBehaviors.range != 0;

        goalDest.target = GameObject.Find("GoalTree").transform;

        setupParms();

        Instantiate(enemyBehaviors.visibleObjectPrefab, this.transform);
        Destroy(transform.Find("Cube").gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("triggered!");
        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            health -= 1f;
            if (health <= 0)
                Destroy(this.gameObject);
        }
    }

    private void setupParms()
    {
        seeker.graphMask = enemyBehaviors.traversableGraphs;
        //ai destination setter target needs to be set
        followerEntity.radius = radius;
        followerEntity.maxSpeed = enemyBehaviors.speed;
        followerEntity.stopDistance = enemyBehaviors.range;
        followerEntity.pathfindingSettings.graphMask = enemyBehaviors.traversableGraphs;

        followerEntity.rvoSettings.layer = ((Pathfinding.RVO.RVOLayer)(1 << (isFlying ? 2 : 3)));
        followerEntity.rvoSettings.collidesWith = ((Pathfinding.RVO.RVOLayer)(1 << (isFlying ? 2 : 3)));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool checkIfFlying()
    {
        return myRecastGraph.walkableClimb > 0.3f;
    }

    private RecastGraph getMyRecastGraph(AstarPath path)
    {
        return (RecastGraph) path.graphs[enemyBehaviors.traversableGraphIndex];
        //need to fix this
        //for (var i = 0; i < 32; i++)
        //{
        //    if (((int)enemyBehaviors.traversableGraphs & 1 << i) == 1)
        //    {
        //        return (RecastGraph) path.graphs[i];
        //    }
        //}
        //throw new System.Exception("Traversible graph not found");
    }
}
