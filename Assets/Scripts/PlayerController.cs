using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EnemyTargeting
{
    public override AttackPriorityOptions type() {
        return AttackPriorityOptions.Player;
    }

    public CharacterController controller;
    public Vector3 direction;
    public float speed = 8;
    public float jumpForce = 10;
    public float gravity = -20;
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public int money;

    Quaternion nonzeroWalkRotation;
    public Transform cameraRotator;
    public Transform cameraRotatorDummy;
    public CameraMovement cameraMovement;

    public bool ableToMakeADoubleJump = true;

    public Transform meshTransform;

    public Animator animator;

    private Transform GetCameraRotation()
    {
        cameraRotatorDummy.eulerAngles = cameraRotator.eulerAngles;
        cameraRotatorDummy.eulerAngles = new Vector3(0, cameraRotator.eulerAngles.y, 0);
        return cameraRotatorDummy;
    }

    private Vector3 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        oldPosition = transform.position;
        cameraMovement = GetComponentInChildren<CameraMovement>();

        GlobalScript.health = 100;
        GlobalScript.maxHealth = 100;
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
                animator.SetTrigger("Jump");
            }

            if (Input.GetKey("1"))
            {
                animator.SetTrigger("Swing");
                print("Swung");

            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;
            if (ableToMakeADoubleJump && Input.GetButtonDown("Jump"))
            {
                direction.y = jumpForce;
                animator.SetTrigger("Jump");
                ableToMakeADoubleJump = false;
            }
        }
        controller.Move(direction * Time.deltaTime);

        animator.SetFloat("VSpeed", direction.y);

        float runMultiplier = (Input.GetKey("right shift")) ? 2f : 1f;

        Vector3 walkDirection = new Vector3();
        //walkDirection.x = hInput * speed;
        //walkDirection.z = vInput * speed;
        Transform currentCameraRot = GetCameraRotation();
        walkDirection += currentCameraRot.right * hInput * speed * runMultiplier;
        walkDirection += currentCameraRot.forward * vInput * speed * runMultiplier;
        controller.Move(walkDirection * Time.deltaTime);

        if (walkDirection.magnitude != 0)
            nonzeroWalkRotation = meshTransform.localRotation;

        animator.SetFloat("Speed", walkDirection.magnitude);

        transform.localEulerAngles = new Vector3(0, 0, 0);
        meshTransform.localRotation = Quaternion.LookRotation(walkDirection);

        if (walkDirection.magnitude == 0)
            meshTransform.localRotation = nonzeroWalkRotation;

        //isGrounded = Physics.CheckSphere(groundCheck.position, 0.09f, groundLayer);
        //isGrounded = Physics.CheckSphere(groundCheck.position, 0.5f, groundLayer);

        isGrounded = Physics.CheckCapsule(new Vector3(groundCheck.position.x, groundCheck.position.y, groundCheck.position.z),
            new Vector3(groundCheck.position.x, groundCheck.position.y - 0.09f, groundCheck.position.z), 0.5f, groundLayer);

        animator.SetBool("Grounded", isGrounded);

        if (transform.position != oldPosition)
            cameraMovement.ExitTowerSelect();
        oldPosition = transform.position;
    }

    public void Build()
    {
        animator.SetTrigger("Build");
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = isGrounded ? Color.green : Color.red;
    //    Gizmos.DrawWireSphere(groundCheck.position, 0.09f);
    //}
}
