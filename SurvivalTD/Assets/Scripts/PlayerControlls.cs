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
        playerInputActions.Player.ScrollHotbarUP.performed += ScrollHotbarUP_performed;
        playerInputActions.Player.ScrollHotbarDown.performed += ScrollHotbarDown_performed;
        playerInputActions.Player.ToggleInventory.performed += ToggleInventory_performed;
        playerInputActions.Player.Escape.performed += Escape_performed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (controllsEnabled)
        {
            HandleMovement();
            HandleLook();
            HandleHotbarNumberKeys();
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
        Vector3 rotation = new Vector3(0, inputVector.x, 0) * mouseSensitivity * Time.deltaTime;
        transform.Rotate(rotation);

        //Camera up/down
        xRotation -= inputVector.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 70);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void ScrollHotbarUP_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerInventory.Instance.ScrollHotBarUp();
    }

    private void ScrollHotbarDown_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        PlayerInventory.Instance.ScrollHotBarDown();
    }

    private void HandleHotbarNumberKeys()
    {
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

    private void ToggleInventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
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

    private void Escape_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (inventoryOn)
        {
            InventoryOff();
        }
    }

    public void HandleEquippment()
    {
        if (controllsEnabled)
        {
            float use = playerInputActions.Player.Equippment_Use.ReadValue<float>();
            float altUse = playerInputActions.Player.Equippment_AltUse.ReadValue<float>();

            if (use > 0)
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

            if (altUse > 0)
            {
                if (controllsEnabled)
                {
                    GameObject activeEquippment = PlayerInventory.Instance.GetActiveEquippment();
                    if (activeEquippment != null)
                    {
                        if (activeEquippment.TryGetComponent(out IEquippment equippment))
                        {
                            equippment.AltUse();
                        }
                    }
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
