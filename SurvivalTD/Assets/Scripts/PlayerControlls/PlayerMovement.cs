using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private float mouseSensitivity;

    float xRotation = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCameraUpDown();
    }

    private void HandleMovement()
    {
        float xMovement = Input.GetAxisRaw("Horizontal");
        float zMovement = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(xMovement, 0, zMovement) * movementSpeed * Time.deltaTime;

        transform.Translate(movement);
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");

        Vector3 rotation = new Vector3(0, mouseX, 0) * mouseSensitivity * 10 * Time.deltaTime;

        transform.Rotate(rotation);
    }

    private void HandleCameraUpDown()
    {
        xRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity / 15;

        xRotation = Mathf.Clamp(xRotation, -90, 70);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

}
