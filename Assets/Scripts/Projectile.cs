using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static SkillTree;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 70f;
    public GameObject impactEffect;
    public float damage = 5f;

    public void Seek(Transform _target)
    {
        target = _target;
    }
    // Start is called before the first frame update
    void Start()
    {
        //damage = damage + skillTree.SkillLevels[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null){
            Destroy(gameObject);
            return;
        }
        
        
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        //if (dir.magnitude <= distanceThisFrame)
        //{
        //    HitTarget();
        //    return;
        //}
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            try
            {
            GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f); //destroy instance after two seconds

            Destroy(this.gameObject);
            other.GetComponent<Enemy>().health -= damage;
            if (other.GetComponent<Enemy>().health <= 0)
                Destroy(other.gameObject);
        }catch (NullReferenceException ex1)
        {
            //This was clogging up Console
            Debug.Log("The targets health is not properly configured!");
        }
    }

    //void HitTarget()
    //{
    //    GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
    //    Destroy(effectIns, 2f); //destroy instance after two seconds
    //    Destroy(target.gameObject); //TEMPORARY REPLACEMENT FOR DAMAGE
    //    Destroy(gameObject);
    //}
    }
}