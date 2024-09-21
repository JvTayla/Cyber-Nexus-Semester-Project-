using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigRobotController : MonoBehaviour
{
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    public float moveSpeed;
    public float lookSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    public Transform playerCamera;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalLookRotation = 0f;
    private Vector3 velocity;
    private CharacterController characterController;

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 20f;

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition;
    private GameObject heldObject;
    public float pickUpRange = 3f;
    private bool holdingGun = false;
    public List<item> availableItems = new List<item>();

    [Header("INVENTORY SETTINGS")]
    [Space(5)]
    public InventoryManage inventoryManage;
    public GameObject inventoryPanel; // Reference to the inventory panel UI
    private bool isInventoryOpen = false; // Track if the inventory is currently open 

    [Header("PUZZLE3 SETTINGS")]
    [Space(5)]
    public bool ToggleSwitch; 
    public GameObject LightOn,LightOff,SwitchOn,SwitchOff;
    public float SwitchRange = 5f;
    public GameObject LightOn2, LightOff2, SwitchOn2, SwitchOff2;


    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public float crouchSpeed = 1.5f;
    public bool isCrouching = false;

    [Header("PUZZLE 1 SETTINGS")]
    [Space(5)]
    public GameObject Player;
    private float tempSpeed;
    private float tempLookAroundSpeed;
    private float tempJumpHeight;


    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        // Set the inventory manager to the instance (make sure it's in the scene)
        if (inventoryManage == null)
        {
            inventoryManage = InventoryManage.Instance;
        }
    }

    private void OnEnable()
    {
        var playerInput = new Controls();
        playerInput.Player.Enable();

        // Input handling
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero;

        playerInput.Player.Jump.performed += ctx => Jump();
        playerInput.Player.Shoot.performed += ctx => Shoot();
        playerInput.Player.PickUp.performed += ctx => PickUpObject();
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch();

        // Handle inventory toggle
        playerInput.Player.Inventory.performed += ctx => ToggleInventory();
        playerInput.Player.Focus.performed += ctx => ToggleLaserSwitch(); 
    } 

    private void Update()
    {
        Move();
        LookAround();
        ApplyGravity();
    }

    public void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);

        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        characterController.Move(move * currentSpeed * Time.deltaTime);
    }

    public void LookAround()
    {
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        transform.Rotate(0, LookX, 0);

        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void Shoot()
    {
        if (holdingGun)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;
            Destroy(projectile, 3f);
        }
    }

    public void PickUpObject()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.transform.parent = null;
            holdingGun = false;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("PickUp"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                holdingGun = true;
            }
            else if (hit.collider.CompareTag("TestTube"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;

                // Add the item to the inventory
                inventoryManage.SpawnItem(availableItems[0]);

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                // Hide the item after picking it up
                heldObject.SetActive(false);
            }
            else if (hit.collider.CompareTag("FireBall"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;

                // Add the item to the inventory
                inventoryManage.SpawnItem(availableItems[1]);

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                // Hide the item after picking it up
                heldObject.SetActive(false);
            }
        }
    }

    public void ToggleCrouch()
    {
        if (isCrouching)
        {
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            characterController.height = crouchHeight;
            isCrouching = true;
        }
    }

    // Function to toggle the inventory panel
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen); // Toggle panel visibility

        if (isInventoryOpen)
        {
            inventoryManage.ListItems(); // Show the items when inventory is open
        }
        else
        {
            inventoryManage.ClearInventoryDisplay(); // Clear the inventory UI when closing
        }
    } 
    public void ToggleLaserSwitch () 
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        Debug.DrawRay(playerCamera.position, playerCamera.forward * SwitchRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, SwitchRange))
        {
            if (hit.collider.CompareTag("Switch"))
            {
                if (ToggleSwitch == true)
                {
                    LightOn.SetActive(true);
                    LightOff.SetActive(false);
                    SwitchOn.SetActive(true);
                    SwitchOff.SetActive(false);
                     
                   
                }
                if (ToggleSwitch == false)
                {
                    LightOn.SetActive(false);
                    LightOff.SetActive(true);
                    SwitchOn.SetActive(false);
                    SwitchOff.SetActive(true); 
                        
                    
                } 


            }
            if (hit.collider.CompareTag("Switch2"))
            {
                if (ToggleSwitch == true)
                {
                    LightOn2.SetActive(true);
                    LightOff2.SetActive(false);
                    SwitchOn2.SetActive(true);
                    SwitchOff2.SetActive(false);


                }
                if (ToggleSwitch == false)
                {
                    LightOn2.SetActive(false);
                    LightOff2.SetActive(true);
                    SwitchOn2.SetActive(false);
                    SwitchOff2.SetActive(true);


                }
            }

           }
        }


    //public Material White;
    //public MeshRenderer console;
    public GameObject consolOff;
    public GameObject consolOn;
    public int consultCount;

    public void Interact()
    {
        // Perform a raycast to detect the lightswitch
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Switch")) // Assuming the switch has this tag
            {
                // Change the material color of the objects in the array
                foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color; // Set the color to match the switch material color
                    }
                }
            }

            else if (hit.collider.CompareTag("Comp"))
            {
                Debug.Log("hit");
                consultCount++;
            }

            /*else if (hit.collider.CompareTag("Door")) // Check if the object is a door
            {
                // Start moving the door upwards
                StartCoroutine(RaiseDoor(hit.collider.gameObject));
            }*/
        }
    }

    private IEnumerator RaiseDoor(GameObject door)
    {
        float raiseAmount = 5f; // The total distance the door will be raised
        float raiseSpeed = 2f; // The speed at which the door will be raised
        Vector3 startPosition = door.transform.position; // Store the initial position of the door
        Vector3 endPosition = startPosition + Vector3.up * raiseAmount; // Calculate the final position of the door after raising

        // Continue raising the door until it reaches the target height
        while (door.transform.position.y < endPosition.y)
        {
            // Move the door towards the target position at the specified speed
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, raiseSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }
    }

}
