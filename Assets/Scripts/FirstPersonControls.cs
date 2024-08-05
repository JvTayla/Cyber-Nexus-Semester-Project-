using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonControls : MonoBehaviour
{
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    private float speedTemp; // Place holder for the original speed 
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump

    public Transform playerCamera; // Reference to the player's camera
    public GameObject laserBeam; // Reference to the laser beam in the game
    public GameObject player; // Reference to the laser beam in the game
    public GameObject clue; // Reference to the laser beam in the game
        
    // Private variables to store input values and the character controller
    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player

    private CharacterController characterController; // Reference to the CharacterController component


    private void Awake()
    {
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
        speedTemp = moveSpeed; // storing the original speed in a temporary variable
    }

    private void OnEnable()//enables input actions , when the script is enables, it starts listening for player inputs , and updates the variables within
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
        playerInput.Player.Shoot.performed += ctx => laserBeam.SetActive(true); // Shows the laser when player shoots
        playerInput.Player.Shoot.canceled += ctx => laserBeam.SetActive(false); // Hides the laser when player doesn't shoot

        playerInput.Player.Sprint.performed += ctx => moveSpeed = speedTemp * 2; //Doubles the speed of the player
        playerInput.Player.Sprint.canceled += ctx => moveSpeed = speedTemp; // Returns the speed to the original speed of the player

    }

    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        IsPlayerNearObject();
    }

    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * moveSpeed * Time.deltaTime);
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

    private float DistanceBetweenObjects(GameObject object1, GameObject object2) // function to get distance between 2 objects
    { 
        //formula : d = sqrt( (x1 - x2)^2 + (y1 - y2)^2 + (z1 - z2)^2) 
        
        var x = object1.transform.position.x - object2.transform.position.x; // the difference between the x positions and storing them in x variable
        var y = object1.transform.position.y - object2.transform.position.y; // the difference between the y positions and storing them in y variable
        var z = object1.transform.position.z - object2.transform.position.z; // the difference between the z positions and storing them in z variable
        
        // the distance formula in 3D space
        var distance =  Mathf.Sqrt(Mathf.Pow( x , 2f ) + Mathf.Pow( x , 2f ) + Mathf.Pow( x , 2f ) ) ;

        return distance ; // return the distance between the objects
    }
    private void IsPlayerNearObject() // function that checks if the player is near the objects
    {
        if (DistanceBetweenObjects(player, clue) > 0 )//checks if the distance is positive between object and player
        {
            // checks if player is more than 10 units away from the object
            if (DistanceBetweenObjects(player, clue) >= 5)
            {
                ClearConsoleWindow();
                Debug.Log("You are not Close"); // hint given to the player
            }
            // checks if the distance between the player and object is between 5 and 10 units
            else if (DistanceBetweenObjects(player, clue) < 4 && DistanceBetweenObjects(player, clue) >= 3 ) 
            {
                ClearConsoleWindow();
                Debug.Log("You are Close");// hint given to the player
            }
            // checks if the distance between the player and object is between 1 and 5 units
            else if (DistanceBetweenObjects(player, clue) < 2 && DistanceBetweenObjects(player, clue) >= 0.2)
            {
                ClearConsoleWindow();
                Debug.Log("You are really Close");
            }
            //checks if the distance between the player and object is positive number and less than 1 unit
            else
            {
                ClearConsoleWindow();
                Debug.Log("You are basically there");
            }
        }
    }
    
    void ClearConsoleWindow()
    {
        // This simply clears the console window
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.SceneView));
        System.Type type = assembly.GetType("UnityEditor.LogEntries");
        System.Reflection.MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (laserBeam.activeSelf) // checks if the game object is active
        {
            laserBeam.transform.parent = other.transform; 
            Debug.Log("Collide");
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (!laserBeam.activeSelf) // checks if the game object is not active
        {
            laserBeam.transform.parent = null;
            Debug.Log("Not Collide");
        }
       
    }
}
