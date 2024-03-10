using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 direction;
    public float speed = 8;
    public float jumpForce = 10;
    public float gravity = -20;
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;

    Quaternion nonzeroWalkRotation;
    public Transform cameraRotator;
    public Transform cameraRotatorDummy;

    public bool ableToMakeADoubleJump = true;

    private Transform GetCameraRotation()
    {
        cameraRotatorDummy.eulerAngles = cameraRotator.eulerAngles;
        cameraRotatorDummy.eulerAngles = new Vector3(0, cameraRotator.eulerAngles.y, 0);
        return cameraRotatorDummy;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        direction.x = 0;
        direction.z = 0;

        if (isGrounded)
        {
            direction.y = -1;
            ableToMakeADoubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                direction.y = jumpForce;
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            if (ableToMakeADoubleJump && Input.GetButtonDown("Jump"))
            {
                direction.y = jumpForce;
                ableToMakeADoubleJump = false;
            }
        }
        controller.Move(direction * Time.deltaTime);

        float runMultiplier = (Input.GetKey("left shift")) ? 2f : 1f;

        Vector3 walkDirection = new Vector3();
        //walkDirection.x = hInput * speed;
        //walkDirection.z = vInput * speed;
        Transform currentCameraRot = GetCameraRotation();
        walkDirection += currentCameraRot.right * hInput * speed * runMultiplier;
        walkDirection += currentCameraRot.forward * vInput * speed * runMultiplier;
        controller.Move(walkDirection * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.09f, groundLayer);
    }
}
