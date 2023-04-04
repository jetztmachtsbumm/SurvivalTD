using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{

    public static PlayerControlls Instance { get; private set; }

    [SerializeField] private float movementSpeed;
    [SerializeField] private float mouseSensitivity;

    private float xRotation = 0;
    private bool inventoryOn;
    private bool controllsEnabled = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("It seems like there is more than one PlayerControlls object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInventory();
        if (controllsEnabled)
        {
            HandleMovement();
            HandleRotation();
            HandleCameraUpDown();
            HandleHotbar();
        }
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

    private void HandleHotbar()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            PlayerInventory.Instance.ScrollHotBarUp();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            PlayerInventory.Instance.ScrollHotBarDown();
        }

        //Number keys
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown((KeyCode)(48 + i)))
            {
                if(i == 0)
                {
                    PlayerInventory.Instance.ChangeHotBarSelectedIndex(10);
                }
                else
                {
                    PlayerInventory.Instance.ChangeHotBarSelectedIndex(i - 1);
                }
            }
        }
    }

    private void HandleInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inventoryOn)
            {
                InventoryOn();
            }
            else
            {
                InventoryOff();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            InventoryOff();
        }
    }

    private void InventoryOn()
    {
        PlayerInventory.Instance.ToggleUI(true);
        inventoryOn = true;
        ToggleControllsOff();
        PlayerInventory.Instance.ChangeHotBarSelectedIndex(-1);
    }

    private void InventoryOff()
    {
        foreach(Inventory inventory in FindObjectsOfType<Inventory>())
        {
            inventory.ToggleUI(false);
        }
        inventoryOn = false;
        ToggleControllsOn();
        PlayerInventory.Instance.ChangeHotBarSelectedIndex(PlayerInventory.Instance.GetHotbarSelectedIndex());
    }

    public void ToggleControllsOn()
    {
        controllsEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ToggleControllsOff()
    {
        controllsEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool GetControllsEnabled()
    {
        return controllsEnabled;
    }

}
