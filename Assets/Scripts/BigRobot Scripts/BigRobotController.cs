using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class BigRobotController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private Controls playerInput;

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
    private float horizontalLookRotaion = 0f;
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
    public TextMeshProUGUI pickUpText;
    private string[] interactableTags = { "PickUp", "TestTube", "Chemicals" };


    [Header("INVENTORY SETTINGS")]
    [Space(5)]
    public InventoryManage inventoryManage;
    public GameObject inventoryPanel; // Reference to the inventory panel UI
    private bool isInventoryOpen = false; // Track if the inventory is currently open 

    [Header("PUZZLE3 SETTINGS")]
    [Space(5)]
    public bool ToggleSwitch;
    public GameObject LightOn, LightOff, SwitchOn, SwitchOff;
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
    private float tempSpeed;//stores a copy of the speed of the robot for later use
    private float tempLookAroundSpeed; // stores a copy of the speed of the mouse speed for later use
    private float tempJumpHeight; // stores a copy of the jump height of the robot for later use

    [Header("UI")] 
    public GameObject SmallRobotUI;
    public GameObject BigRobotUI;

    private bool isMenuMode = true;

    private HealthScript _HealthScript;
    private BIgRobotHeadBobbingHead _BigRobotHeadBobbingHead;
    private SwitchCameraAnimationScript _CameraAnimation;
    private CorePowerScript _CorePowerScript;



    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color

    private void Awake()
    {
         //Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
        _HealthScript = FindObjectOfType<HealthScript>();
        _CameraAnimation = FindObjectOfType<SwitchCameraAnimationScript>();
        
        _BigRobotHeadBobbingHead = FindObjectOfType<BIgRobotHeadBobbingHead>();
        _CorePowerScript = FindObjectOfType<CorePowerScript>();
        // Set the inventory manager to the instance (make sure it's in the scene)
        if (inventoryManage == null)
        {
            inventoryManage = InventoryManage.Instance;
        }

        playerInput = new Controls();
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions        

        // Enable the input actions
        playerInput.Player.Enable();
        // UiInput.UI.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled
        
        //playerInput.Player.Movement.performed += ctx => _BigRobotHeadBobbingHead.StartBobbing();
       // playerInput.Player.Movement.canceled += ctx => _BigRobotHeadBobbingHead.StopBobbing();
        
        
        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed
        
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
        playerInput.Player.Interact.performed += ctx => ToggleLaserSwitch(); // press F

        playerInput.Player.Pause.performed += ctx => PauseGame();
        playerInput.Player.SwitchRobot.performed += ctx => SwitchToWisp();


        //UiInput.UI.Navigate.performed += ctx => NavigateUI(ctx.ReadValue<Vector2>());
        //UiInput.UI.Submit.performed += ctx => SubmitUI();
        //UiInput.UI.Cancel.performed += ctx => CancelUI();
    }
       
    
//move to Robot controller
    private void SwitchToWisp()
    {
        //_CameraAnimation.SwitchToSmallRobot();
        _CorePowerScript.BigRobotUI.SetActive(false);
        _CorePowerScript.SmallRobotUI.SetActive(true);
        _HealthScript.IsBigRobotInControl = false;
        
        if (!_HealthScript.IsBigRobotInControl)
        {
            _CorePowerScript.BigRobotHideDeadScreen();
            _CorePowerScript.StopBigRobotWarning();
            _CorePowerScript.BigRobotUI.SetActive(false);
        }
        
    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        CheckForPickUp();
    }

    
    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        //Adjust speed if crouching
        float currentSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * (currentSpeed * Time.deltaTime));
    }

    public void PauseGame()
    {
        playerInput.Player.Disable();
        playerInput.PauseMenu.Enable();
        pauseMenuUI.SetActive(true);
    }


    public void ResumeScreen()
    {
        playerInput.PauseMenu.Disable();
        playerInput.Player.Enable();
        pauseMenuUI.SetActive(false);
    } 
    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
       
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
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
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false; // Enable physics
            heldObject.transform.parent = null;
            holdingGun = false;
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);


        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                holdingGun = true;
            }
            else
            if (hit.collider.CompareTag("TestTube"))
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
            else if (hit.collider.CompareTag("Chemicals"))
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
            else if (hit.collider.CompareTag("VoiceRecorder"))
            {
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true;

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                
                GameObject VoiceRecrod =  hit.collider.transform.GetChild(0).gameObject;
                VoiceRecrod.SetActive(true);
               
            }
        }
    }

    public void ToggleCrouch()
    {
        if (isCrouching)
        {
            //Stand up
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            //Crouch down
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
            playerInput.Inventory.Enable();
            playerInput.Player.LookAround.Disable(); 
            playerInput.Player.Movement.Disable();
           
        }
        else
        {
            inventoryManage.ClearInventoryDisplay(); // Clear the inventory UI when closing   
            playerInput.Inventory.Disable();
            playerInput.Player.LookAround.Enable();
            playerInput.Player.Movement.Enable();

        }
    }
    public void ToggleLaserSwitch()
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
                
               /* foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color; // Set the color to match the switch material color
                    }
                }*/
            }

            else if (hit.collider.CompareTag("Comp"))
            {
                Debug.Log("hit");
                consultCount++;
            }
            else if (hit.collider.CompareTag("Door")) // Check if the object is a door
            {
                // Start moving the door upwards
                StartCoroutine(RaiseDoor(hit.collider.gameObject));
            }
        }
    }

    private void CheckForPickUp()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Perform raycast to detect objects
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the object has any of the specified tags
            if (IsInteractable(hit.collider.tag))
            {
                // Display the pick-up text
                pickUpText.gameObject.SetActive(true);
                
                pickUpText.text = hit.collider.gameObject.name;
            }
            else
            {
                // Hide the pick-up text if not looking at an interactable object
                pickUpText.gameObject.SetActive(false);
               
            }
        }
        else
        {
            // Hide the text if not looking at any object
            pickUpText.gameObject.SetActive(false);
        }
    }
    // Helper function to check if the object's tag is in the interactableTags list
    private bool IsInteractable(string objectTag)
    {
        foreach (string tag in interactableTags)
        {
            if (objectTag == tag)
            {
                return true;
            }
        }
        return false;
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





   