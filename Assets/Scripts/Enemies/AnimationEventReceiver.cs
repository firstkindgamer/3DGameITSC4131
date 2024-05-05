using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReciever : MonoBehaviour
{
    private Enemy enemyParent;

    public void fireBullet()
    {
        enemyParent.fireBullet();
    }

    void Start()
    {
        enemyParent = transform.parent.GetComponent<Enemy>();
    }
}
