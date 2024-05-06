using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TowerPlacer : MonoBehaviour
{
    CameraMovement cameraMovement;
    Camera camera;
    public LayerMask groundLayer;
    public Tower selectedTower;

    MeshRenderer meshRenderer;
    public GameObject towerToPlace;

    PlayerController player;

    public RectTransform optionsPos;

    public RaycastHit findHighestSurface(float x, float z)
    {
        RaycastHit hit;
        Physics.CapsuleCast(new Vector3(x, 50, z), new Vector3(x, 51, z), 1.443531f, Vector3.down, out hit, Mathf.Infinity, groundLayer);
        //return new Vector3(x, hit.point.y, z);
        return hit;
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = GameObject.FindObjectOfType<CameraMovement>();
        camera = GameObject.FindObjectOfType<Camera>();

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
                //print(hit.collider.gameObject.name);
            }

            RaycastHit capsuleHit = findHighestSurface(transform.position.x, transform.position.z);
            if (capsuleHit.collider.gameObject.name == "GroundCollider")
            {
                //meshRenderer.enabled = false;
                transform.localScale = new Vector3(1.443531f, 0f, 1.443531f);
            }
            else
            {
                //meshRenderer.enabled = true;
                transform.localScale = new Vector3(2.887062f, 0, 2.887062f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                //optionsPos.position = camera.WorldToScreenPoint(hit.point);

                if (capsuleHit.collider.gameObject.name == "GroundCollider")
                {
                    cameraMovement.mouseMode = MouseMode.TowerSelect;

                    selectedTower = capsuleHit.collider.gameObject.transform.parent.GetComponent<Tower>();

                    Vector3 screenPoint = camera.WorldToScreenPoint(capsuleHit.collider.transform.position);
                    optionsPos.position = screenPoint;
                    Mouse.current.WarpCursorPosition(screenPoint);

                } else
                {
                    GameObject newTower = Instantiate(towerToPlace, new Vector3(transform.position.x, capsuleHit.point.y, transform.position.z), Quaternion.identity);
                    player.Build();
                }
                //newTower.transform.position = findHighestSurface(transform.position.x, transform.position.z);
            }
        } else
        {
            meshRenderer.enabled = false;
        }
    }

    public TMP_Text upgradeText;
    public TMP_Text salvageText;
    public TMP_Text toggleText;
    public TMP_Text groundAirText;

    public void Upgrade()
    {
        selectedTower.Upgrade();
        upgradeText.text = selectedTower.UpgradeText();
    }
    public void Salvage()
    {
        selectedTower.Salvage();
        salvageText.text = selectedTower.SalvageText();
    }
    public void Toggle()
    {
        selectedTower.Toggle();
        toggleText.text = selectedTower.ToggleText();
    }
    public void GroundAir()
    {
        selectedTower.GroundAir();
        groundAirText.text = selectedTower.GroundAirText();
    }

    public void SetAllText()
    {
        upgradeText.text = selectedTower.UpgradeText();
        salvageText.text = selectedTower.SalvageText();
        toggleText.text = selectedTower.ToggleText();
        groundAirText.text = selectedTower.GroundAirText();
    }
}
