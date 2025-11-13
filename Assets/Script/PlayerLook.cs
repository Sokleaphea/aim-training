using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerLook : MonoBehaviour
{
    public float sensitivity = 2f; // mouse sensitivity
    public Transform playerBody;   // assign Player root here
    private float rotationX = 0f;  // vertical rotation (pitch)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Vertical rotation (look up/down)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Horizontal rotation (look left/right)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
