using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{

    public static PlayerControlls Instance { get; private set; }

    [SerializeField] private float movementSpeed;
    [SerializeField] private float mouseSensitivity;

    private PlayerInputActions playerInputActions;
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

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleInventory();
        if (controllsEnabled)
        {
            HandleMovement();
            HandleLook();
            HandleHotbar();
            HandleEquippment();
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y) * movementSpeed * Time.deltaTime;

        transform.Translate(movement);
    }

    private void HandleLook()
    {
        //Left Right
        Vector2 inputVector = playerInputActions.Player.Look.ReadValue<Vector2>();
        Vector3 rotation = new Vector3(0, inputVector.x, 0) * mouseSensitivity * 10 * Time.deltaTime;
        transform.Rotate(rotation);

        //Camera up/down
        xRotation -= inputVector.y * mouseSensitivity / 15;
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

    private void HandleEquippment()
    {
        if (Input.GetMouseButton(0))
        {
            GameObject activeEquippment = PlayerInventory.Instance.GetActiveEquippment();
            if (activeEquippment != null)
            {
                if (activeEquippment.TryGetComponent(out IEquippment equippment))
                {
                    equippment.Use();
                }
            }
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
