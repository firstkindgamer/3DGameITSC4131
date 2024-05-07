using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSwarm : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Destroy(gameObject);
        }
    }
}
