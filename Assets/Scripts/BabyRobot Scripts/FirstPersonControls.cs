using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class FirstPersonControls : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private Controls playerInput;
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
                                   // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private GameObject heldObject; // Reference to the currently held object
    public float pickUpRange = 3f; // Range within which objects can be picked up
    private bool holdingGun = false;
    public item Item;
    
    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1f; //make short
    public float standingHeight = 2f; //make normal
    public float crouchSpeed = 1.5f; //make slow
    public bool isCrouching = false; //check if crouch


    [Header("PUZZLE 1 SETTINGS")]
    [Space(5)]
    public GameObject Player;//Gameobject of the small robot
    public Camera babyRobotCamera;//Camera of the small robot
    private PuzzleScript _PuzzleScript;// Reference to the puzzle script
    
    public float tempSpeed;//stores a copy of the speed of the robot for later use
    public float tempLookAroundSpeed; // stores a copy of the speed of the mouse speed for later use
    public float tempJumpHeight; // stores a copy of the jump height of the robot for later use
    
    private ColorChangerScript _ColorChangerScript; //reference to the colorchangescript
    private SmallRobotHeadBobbing _SmallRobotHeadBobbing;

    [Header("INTERACT SETTINGS")]
    [Space(5)]
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color
    
    [Header("UI")] 
    public GameObject SmallRobotUI;
    public GameObject BigRobotUI;

    [Header("UI SETTINGS")]
    public TextMeshProUGUI pickUpText;
    public Image healthBar;
    public float damageAmount = 0.25f; // Reduce the health bar by this amount
    private float healAmount = 0.5f;// Fill the health bar by this amount

    private HealthScript _HealthScript;
    private SwitchCameraAnimationScript _CameraAnimation;
    private CorePowerScript _CorePowerScript;
    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
        
        //Gets all the public functions and variables in the puzzlescript
        _PuzzleScript = FindObjectOfType<PuzzleScript>();
        
        //store the robot data in temp data for later use
        tempSpeed = moveSpeed;
        tempLookAroundSpeed = lookSpeed;
        tempJumpHeight = jumpHeight;

        //Get all the public functions and variables in the script
        _ColorChangerScript = FindObjectOfType<ColorChangerScript>();
        _SmallRobotHeadBobbing = FindObjectOfType<SmallRobotHeadBobbing>();
        _HealthScript = FindObjectOfType<HealthScript>();
        _CameraAnimation = FindObjectOfType<SwitchCameraAnimationScript>();
        _CorePowerScript = FindObjectOfType<CorePowerScript>();

         playerInput = new Controls();

    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        

        // Enable the input actions
        playerInput.Player.Enable();
        
        if (!_CorePowerScript.SmallRobotDead)
        {
                    // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled
        
        playerInput.Player.Movement.performed += ctx => _SmallRobotHeadBobbing.StartBobbing();
        playerInput.Player.Movement.canceled += ctx => _SmallRobotHeadBobbing.StopBobbing();
        
        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        // Subscribe to the crouch input event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); // Call the ToggleCrouch method when crouch input is performed

        // Subscribe to the interact input event
        playerInput.Player.Focus.performed += ctx => _PuzzleScript.IntertactWithObject(); // Call the Interact method when interact input is performed
        playerInput.Player.Focus.canceled += ctx => _PuzzleScript.StopInteracting();// Reset Inteact method when interact is canceled

        playerInput.Player.TileSelector.performed += ctx => _PuzzleScript.InteractWithPuzzle();
        
        // Subscribe to the interact input event
        playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch 

        playerInput.Player.Pause.performed += ctx => PauseGame();

        }

        playerInput.Player.SwitchRobot.performed += ctx => SwitchToAurora();
        
    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
    }
    
    private void SwitchToAurora()
    {
        // _CameraAnimation.SwitchToBigRobot();
        BigRobotUI.SetActive(true);
        SmallRobotUI.SetActive(false);
        _HealthScript.IsBigRobotInControl = true;

        if (_HealthScript.IsBigRobotInControl)
        {
            _CorePowerScript.SmallRobotHideDeadScreen();
            _CorePowerScript.StopSmallRobotWarning();
            _CorePowerScript.SmallRobotUI.SetActive(false);
            
        }
        
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
            // Calculate the jump velocity
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
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics
                InventoryManage.Instance.SpawnItem(Item);
                
                
                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
                
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
    private void CheckForPickUp()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        // Perform raycast to detect objects
        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the object has the "PickUp" tag
            if (hit.collider.CompareTag("PickUp"))
            {
                // Display the pick-up text
                pickUpText.gameObject.SetActive(true);
                pickUpText.text = hit.collider.gameObject.name;
            }
            else
            {
                // Hide the pick-up text if not looking at a "PickUp" object
                pickUpText.gameObject.SetActive(false);
            }
        }
        else
        {
            // Hide the text if not looking at any object
            pickUpText.gameObject.SetActive(false);
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
    public void PauseGame()
    {
        playerInput.Player.Disable();
        playerInput.PauseMenu.Enable();
        pauseMenuUI.SetActive(true);
    }


    public void ResumeScreenBby()
    {
        playerInput.PauseMenu.Disable();
        playerInput.Player.Enable();
        pauseMenuUI.SetActive(false);
    }

}



