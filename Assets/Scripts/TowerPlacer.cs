using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    CameraMovement cameraMovement;
    public LayerMask groundLayer;

    MeshRenderer meshRenderer;
    public GameObject towerToPlace;

    PlayerController player;

    public Vector3 findHighestSurface(float x, float z)
    {
        RaycastHit hit;
        Physics.CapsuleCast(new Vector3(x, 50, z), new Vector3(x, 51, z), 1.443531f, Vector3.down, out hit, Mathf.Infinity, groundLayer);
        return new Vector3(x, hit.point.y, z);
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = GameObject.FindObjectOfType<CameraMovement>();

        meshRenderer = GetComponent<MeshRenderer>();

        player = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMovement.mouseMode == MouseMode.TowerPlace)
        {
            meshRenderer.enabled = true;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                transform.position = hit.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                GameObject newTower = Instantiate(towerToPlace, findHighestSurface(transform.position.x, transform.position.z), Quaternion.identity);
                player.Build();
                //newTower.transform.position = findHighestSurface(transform.position.x, transform.position.z);
            }
        } else
        {
            meshRenderer.enabled = false;
        }
    }
}
