using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var renderers = gameObject.GetComponentsInChildren<Renderer>();
        var bounds = renderers[0].bounds;
        for (var i = 1; i < renderers.Length; ++i)
            bounds.Encapsulate(renderers[i].bounds);
        print("bounds: " + bounds.size);
    }
}
