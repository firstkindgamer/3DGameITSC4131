using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Linq;

public abstract class EnemyTargeting : MonoBehaviour
{
    public abstract AttackPriorityOptions type();
    public Transform Transform { get; } 
    public float additionalStoppingRadius() { return 1f; } //should be the radius of whatever it is
}

public class Enemy : MonoBehaviour
{
    private Tuple<EnemyTargeting, EnemyTargeting> getPrimaryAndSecondaryTargets()
    {
        List<EnemyTargeting> possibleTargets = FindObjectsOfType<EnemyTargeting>().ToList();
        print(possibleTargets.Count);
        for (int i = possibleTargets.Count - 1; i >= 0; i--)
        {
            if (!enemyBehaviors.attackPriority.Contains(possibleTargets[i].type()))
                possibleTargets.RemoveAt(i);
        }
        possibleTargets.Sort(SortByPriority);
        print(possibleTargets.Count);
        while (possibleTargets.Count < 2) possibleTargets.Add(null);
        return new Tuple<EnemyTargeting, EnemyTargeting>(possibleTargets[0], possibleTargets[1]);
    }
    private int SortByPriority(EnemyTargeting t1, EnemyTargeting t2)
    {
        int t1Priority = enemyBehaviors.attackPriority.IndexOf(t1.type());
        int t2Priority = enemyBehaviors.attackPriority.IndexOf(t2.type());
        int priorityCompare = t1Priority.CompareTo(t2Priority);
        if (priorityCompare != 0)
            return priorityCompare;
        float t1Distance = Vector3.Distance(transform.position, t1.Transform.position);
        float t2Distance = Vector3.Distance(transform.position, t2.Transform.position);
        return t1Distance.CompareTo(t2Distance);
    }

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
    private Animator animator;

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

        setupParms();

        visibleObject = Instantiate(enemyBehaviors.visibleObjectPrefab, this.transform);
        Destroy(transform.Find("Cube").gameObject);
        animator = visibleObject.GetComponent<Animator>();

        if (!isFlying)
            visibleObject.transform.localPosition = new Vector3(0, 0, 0); //this has to be here or it will go off its rails

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
        //followerEntity.stopDistance = enemyBehaviors.range; //done in update
        followerEntity.pathfindingSettings.graphMask = enemyBehaviors.traversableGraphs;

        followerEntity.rvoSettings.layer = ((Pathfinding.RVO.RVOLayer)(1 << (isFlying ? 2 : 3)));
        followerEntity.rvoSettings.collidesWith = ((Pathfinding.RVO.RVOLayer)(1 << (isFlying ? 2 : 3)));
    }

    // Update is called once per frame
    void Update()
    {
        goalDest.target = GameObject.Find("Player").transform;

        Tuple <EnemyTargeting, EnemyTargeting> targets = getPrimaryAndSecondaryTargets();
        //print(targets.Item1.name);
        //print(targets.Item1.gameObject.transform.position);

        goalDest.target = targets.Item1.gameObject.transform;
        followerEntity.stopDistance = getStoppingDistance(targets.Item1);

        float distanceMovedSinceLastFrame = Vector3.Distance(transform.position, positionLastFrame);

        animator.SetBool("isMoving", distanceMovedSinceLastFrame > 0.01f); //this may need to be adjusted to insure the walk animation stops when standing still

        positionLastFrame = transform.position;
    }

    private Vector3 positionLastFrame;

    private void Start()
    {
        positionLastFrame = transform.position;
    }

    private float getStoppingDistance(EnemyTargeting targ)
    {
        return radius + targ.additionalStoppingRadius() + enemyBehaviors.range;
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