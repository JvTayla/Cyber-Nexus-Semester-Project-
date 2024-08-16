using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRobotController : MonoBehaviour
{
  

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

    [Header("CROUCH SETTINGS")]
    [Space(5)]
    public float crouchHeight = 1f; //make short
    public float standingHeight = 2f; //make normal
    public float crouchSpeed = 1.5f; //make slow
    public bool isCrouching = false; //check if crouch

    [Header("PUSHING UP SETTINGS")]
    [Space(5)]
    
    private GameObject PushableObject; // Reference to the currently held object
    public float PushableRange = 3f; // Range within which objects can be picked up
    private bool isPushing = false;

    public GameObject Player;
    public Camera bigRobotCamera;
    private PuzzleScript _PuzzleScript;// Reference to the puzzle script
    
    private float tempSpeed;//stores a copy of the speed of the robot for later use
    private float tempLookAroundSpeed; // stores a copy of the speed of the mouse speed for later use
    private float tempJumpHeight; // stores a copy of the jump height of the robot for later use
    private ColorChangerScript _ColorChangerScript; //reference to the colorchangescript
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

        //Get all the public functions and variables in the Colorchangescript
        _ColorChangerScript = FindObjectOfType<ColorChangerScript>();
    }

    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

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
        playerInput.Player.Interact.performed += ctx => IntertactWithObject(); // Call the Interact method when interact input is performed
        playerInput.Player.Interact.canceled += ctx => StopInteracting();// Reset Inteact method when interact is canceled


        playerInput.Player.TileSelector.performed += ctx => InteractWithPuzzle();
    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
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
        characterController.Move(move * currentSpeed * Time.deltaTime);
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
    
    //function that that robot uses to interact with interactible objects ( Code soon to be changed because of other interactable objects)
    private void IntertactWithObject()
    {
        //checks if the puzzle is not complete to continue
        if (!_PuzzleScript.IsPuzzleComplete())
        {
            // Perform a raycast from the camera's position forward
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;
            
            // Debugging: Draw the ray in the Scene view
            Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);
            
            
            if (Physics.Raycast(ray, out hit, pickUpRange * 2))
            {
                //checks if the raycast hits the objects with the tags shown below
                if (hit.collider.CompareTag("Interactable") || hit.collider.CompareTag("OtherTiles") ||
                    hit.collider.CompareTag("StraightTiles"))
                {
                    //spot stores the transform of the hit that the raycast hit
                    Transform spot = hit.collider.transform;
                    
                    //we now make the position of the player to the position of the spot
                    Player.transform.position = spot.position;

                    //we now also make the rotation of the player to the rotation of the spot
                    var angles = Player.transform.eulerAngles;
                    angles.y = 90f;
                    Player.transform.eulerAngles = angles;
                    
                    //we know do assign the data of the robot to stop so it can focus on the object in hand
                    moveSpeed = 0;
                    jumpHeight = 0;
                    lookSpeed = 0;
                }
            }
        }
        else if (_PuzzleScript.IsPuzzleComplete())//if the player solved the puzzle do this
        {
            //function makes the player stop interacting with the object
            StopInteracting();
            
            //function opens the doors after solving the puzzle
            _PuzzleScript.DoorOpener();
            _ColorChangerScript.MeshRenderer.materials[0].color = Color.green;
        }
        
    }
    
    private void InteractWithPuzzle()
    {
        //checks if the puzzle is already solved or not
        if (!_PuzzleScript.IsPuzzleComplete())
        {
            //makes a raycase from the baby camera where the mouse is pointing
            Ray ray = bigRobotCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            // Debugging: Draw the ray in the Scene view
            Debug.DrawRay(ray.origin, ray.direction *100f, Color.green, 2f);

            
            if (Physics.Raycast(ray, out hit, pickUpRange*2))
            {
                //checks if the hit of the raycast has a tag of the specific tags shown below
                if (hit.collider.CompareTag("OtherTiles") || hit.collider.CompareTag("StraightTiles"))
                {
                    //store the hit of the raycast in the tile gameobject
                    GameObject tile = hit.collider.gameObject;
                    
                    //rotate the tile by 90 degrees
                    var angles = tile.transform.eulerAngles;
                    angles.z -= 90f;
                    tile.transform.eulerAngles = angles;
                }
            } 
        }
        else if (_PuzzleScript.IsPuzzleComplete()) //if the player solves the player they can move now
        {
            moveSpeed = tempSpeed;
            jumpHeight = tempJumpHeight;
            lookSpeed = tempLookAroundSpeed;
            
            // function opens the door
            _PuzzleScript.DoorOpener();
            
            //makes the puzzle background green to show the puzzle is complete
            _ColorChangerScript.MeshRenderer.materials[0].color = Color.green;
        }
    }
    
    //function that allows the player to start moving again
    private void StopInteracting()
    {
        
        moveSpeed = tempSpeed;
        jumpHeight = tempJumpHeight;
        lookSpeed = tempLookAroundSpeed;
    }

    /*public void PushObjects()
    {


        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * PushableRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, PushableRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("Pushable"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                //
                Debug.Log("Pushable Object");

            }
          
        }
    }*/


}

