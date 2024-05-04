using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    private static float FLIGHT_HEIGHT = 7f;

    private Seeker seeker;
    public AIDestinationSetter goalDest;
    private FollowerEntity followerEntity;
    public RecastGraph myRecastGraph;

    public bool isFlying;
    public float radius;
    public bool isRanged;

    public float health = 10;

    public EnemyBehaviorScriptableObject enemyBehaviors;

    private SphereCollider sphereCollider;
    public Transform target; //this is where bullets should aim at

    private GameObject visibleObject;

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

        visibleObject = Instantiate(enemyBehaviors.visibleObjectPrefab, this.transform);
        Destroy(transform.Find("Cube").gameObject);

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = radius;
        sphereCollider.center = new Vector3(0, radius, 0);
        if (isFlying)
        {
            visibleObject.transform.localPosition = new Vector3(0, FLIGHT_HEIGHT, 0);
            sphereCollider.center = new Vector3(0, sphereCollider.center.y + FLIGHT_HEIGHT, 0);
        }
        target = transform.Find("Target");
        target.transform.localPosition = new Vector3(0, transform.position.y + sphereCollider.center.y, 0);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print("triggered!");
    //    if (other.tag == "Bullet")
    //    {
    //        Destroy(other.gameObject);
    //        health -= 1f;
    //        if (health <= 0)
    //            Destroy(this.gameObject);
    //    }
    //}

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
