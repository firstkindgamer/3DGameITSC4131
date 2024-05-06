using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Linq;

public abstract class EnemyTargeting : MonoBehaviour
{
    public abstract AttackPriorityOptions type();
    public float additionalStoppingRadius() { return 1f; } //should be the radius of whatever it is
    public float health = 5f;
    public void changeHealth(float amount)
    {
        health -= amount;
        print("health reduced");
    }
}

public class Enemy : MonoBehaviour
{
    private Tuple<EnemyTargeting, EnemyTargeting> getPrimaryAndSecondaryTargets()
    {
        List<EnemyTargeting> possibleTargets = FindObjectsOfType<EnemyTargeting>().ToList(); //could optimize by having all enemytargeting objects mantain a single list of themselves
        print(possibleTargets.Count);
        for (int i = possibleTargets.Count - 1; i >= 0; i--)
        {
            if (!enemyBehaviors.attackPriority.Contains(possibleTargets[i].type()))
                possibleTargets.RemoveAt(i);
        }
        possibleTargets.Sort(SortByPriority);

        EnemyTargeting mainTarget;
        if (possibleTargets.Count > 0)
            mainTarget = possibleTargets[0];
        else
            mainTarget = null;

        for (int i = possibleTargets.Count - 1; i >= 0; i--)
        {
            if (distanceToTarget(possibleTargets[i]) >= getShootingDistance(possibleTargets[i]))
                possibleTargets.RemoveAt(i);
        }
        possibleTargets.Sort(SortByPriority);

        EnemyTargeting secondaryTarget;
        if (possibleTargets.Count > 0)
            secondaryTarget = possibleTargets[0];
        else
            secondaryTarget = null;

        return new Tuple<EnemyTargeting, EnemyTargeting>(mainTarget, secondaryTarget);
    }
    private int SortByPriority(EnemyTargeting t1, EnemyTargeting t2)
    {
        int t1Priority = enemyBehaviors.attackPriority.IndexOf(t1.type());
        int t2Priority = enemyBehaviors.attackPriority.IndexOf(t2.type());
        int priorityCompare = t1Priority.CompareTo(t2Priority);
        if (priorityCompare != 0)
            return priorityCompare;
        float t1Distance = distanceToTarget(t1);
        float t2Distance = distanceToTarget(t2);
        return t1Distance.CompareTo(t2Distance);
    }

    //public void printList(List<EnemyTargeting> myList) //for debugging
    //{
    //    string result = "List contents: ";
    //    foreach (var item in myList)
    //    {
    //        result += item.ToString() + "|| ";
    //    }
    //    Debug.Log(result);
    //}

    private static float FLIGHT_HEIGHT = 7f;

    public GameObject enemyBulletPrefab;
    private Transform firePoint;

    private Seeker seeker;
    public AIDestinationSetter goalDest;
    public FollowerEntity followerEntity;
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
    public static Enemy enemy; //Hi max dont mind me
    public void Awake()
    {
        enemy = this;
    }

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
        animator.SetBool("isRanged", isRanged);
        animator.SetFloat("attackSpeed", enemyBehaviors.attackRate / (isRanged ? 2.3f : 1.5f)); //these are the lengths of the attack clips to get them to be 1/sec

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

        firePoint = GetChildGameObject(visibleObject.gameObject, "FirePoint").transform;

        if (isFlying)
        {
            InvokeRepeating("fireBullet", 0, enemyBehaviors.attackRate); //no animator attack, so do this
        }
        if(isFlying)
        {
            this.tag = "Air";
        } else this.tag = "Ground";
    }

    public GameObject GetChildGameObject(GameObject fromGameObject, string withName)
    {
        var allKids = fromGameObject.GetComponentsInChildren<Transform>();
        var kid = allKids.FirstOrDefault(k => k.gameObject.name == withName);
        if (kid == null) return null;
        return kid.gameObject;
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

    Tuple<EnemyTargeting, EnemyTargeting> targets;

    // Update is called once per frame
    void Update()
    {
        //goalDest.target = GameObject.Find("Player").transform;

        targets = getPrimaryAndSecondaryTargets(); //could optimize by making an event only change when new tower created or destroyed
        //print(targets.Item1.name);
        //print(targets.Item1.gameObject.transform.position);

        goalDest.target = targets.Item1.gameObject.transform;
        followerEntity.stopDistance = getStoppingDistance(targets.Item1);

        float distanceMovedSinceLastFrame = Vector3.Distance(transform.position, positionLastFrame);
        animator.SetBool("isMoving", distanceMovedSinceLastFrame > 0.01f); //this may need to be adjusted to insure the walk animation stops when standing still
        positionLastFrame = transform.position;

        if (targets.Item1 != null && (distanceToTarget(targets.Item1) < getShootingDistance(targets.Item1)))
        {
            animator.SetBool("isAttacking", true); //animator doesnt need to be set for flying enemies, but doing so doesnt do any errors
            currentAttackingTarget = targets.Item1;
        } else if (targets.Item2 != null && distanceToTarget(targets.Item2) < getShootingDistance(targets.Item2))
        {
            animator.SetBool("isAttacking", true);
            currentAttackingTarget = targets.Item2;
        } else
        {
            animator.SetBool("isAttacking", false);
            currentAttackingTarget = null;
        }
    }

    EnemyTargeting currentAttackingTarget;

    public float distanceToTarget(EnemyTargeting enemy)
    {
        return Vector3.Distance(firePoint.position, enemy.gameObject.transform.position);
    }

    public void fireBullet()
    {
        if (currentAttackingTarget == null) return;

        if (isRanged)
        {
            EnemyBullet b = Instantiate(enemyBulletPrefab).GetComponent<EnemyBullet>();
            b.gameObject.transform.position = firePoint.position;
            b.enemyBehaviors = enemyBehaviors;
            b.maxTravelDistance = getShootingDistance(currentAttackingTarget);

            //use getShootingDistance for this bullet's range, NOT from enemyBehaviors, as flying enemies need a boost

            //b.target = currentAttackingTarget.gameObject.transform.position;
            b.direction = (currentAttackingTarget.transform.position - firePoint.position).normalized;
            b.startingPoint = firePoint.position;
        } else
        {
            currentAttackingTarget.health -= enemyBehaviors.damage;
        }
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
    private static float ATTACK_BUFFER_ROOM = 1f; //1f for buffer room to attack
    private float getShootingDistance(EnemyTargeting targ)
    {
        //use pythagorean theorem here with fly height for flying enemies, they should shoot a bit further
        //if (isFlying)
        //{
        //    float groundStoppingDistance = getStoppingDistance(targ);
        //    return Mathf.Sqrt(Mathf.Pow(groundStoppingDistance, 2) + Mathf.Pow(FLIGHT_HEIGHT, 2)) + ATTACK_BUFFER_ROOM;
        //} else
        //    return getStoppingDistance(targ) + ATTACK_BUFFER_ROOM;

        float groundStoppingDistance = getStoppingDistance(targ);
        return Mathf.Sqrt(Mathf.Pow(groundStoppingDistance, 2) + Mathf.Pow(target.transform.localPosition.y, 2)) + ATTACK_BUFFER_ROOM;
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