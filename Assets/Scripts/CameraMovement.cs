using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public enum MouseMode
{
    Camera,
    TowerPlace,
    Upgrades
}

public class CameraMovement : MonoBehaviour
{
    GameObject camera;
    //Vector2 cursorPos = new Vector2(Screen.width / 2, Screen.height / 2);
    public Transform defaultMousePos;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public MouseMode mouseMode = MouseMode.Camera;

    private void Start()
    {
        camera = transform.Find("Main Camera").gameObject;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseMode = MouseMode.Camera;
    }

    void Update()
    {
        if (mouseMode == MouseMode.Camera)
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        MouseMode oldMouseMode = mouseMode;

        if (Input.GetKeyDown("f"))
        {
            if (mouseMode == MouseMode.Camera || mouseMode == MouseMode.TowerPlace)
                mouseMode = MouseMode.Upgrades;
            else if (mouseMode == MouseMode.Upgrades)
            {
                if (Input.GetKey("left shift"))
                    mouseMode = MouseMode.TowerPlace;
                else
                    mouseMode = MouseMode.Camera;
            }
        }

        if (mouseMode != MouseMode.Upgrades)
        {

            if (Input.GetKeyDown("left shift"))
            {
                //Mouse.current.WarpCursorPosition(cursorPos);
                mouseMode = MouseMode.TowerPlace;
            }
            else if (Input.GetKeyUp("left shift"))
            {
                //cursorPos = Input.mousePosition;
                mouseMode = MouseMode.Camera;
            }

        }

        if (mouseMode != oldMouseMode)
        {
            switch (mouseMode)
            {
                case MouseMode.Upgrades:
                    Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case MouseMode.TowerPlace:
                    Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(defaultMousePos.position));
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                case MouseMode.Camera:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;

            }
        }

    }

}