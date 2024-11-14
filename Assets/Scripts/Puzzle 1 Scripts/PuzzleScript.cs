using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class PuzzleScript : MonoBehaviour
{
    public GameObject puzzleScreen; // reference to the puzzle screen
    private GameObject[] arrAllTiles; //array that will store all the tiles gameobjects
    private GameObject[] arrAllCorrectTiles; // array that will store all the correct pipe tiles gameobjects
    public GameObject NumberPad;
    public GameObject[] openDoor;
    public GameObject DoorTrigger;

    public bool PuzzleSolved;

    private FirstPersonControls _FirstPersonControls;

    private ColorChangerScript _ColorChangerScript;

   // private RedBlinkingLights _RedBlinkingLights;

    private Alarm Alarm;

    private AnimationScript _AnimationScript;

    private SoundScript _SoundScript;
    // Start is called before the first frame update
    void Start()
    {
        PuzzleSolved = false;
        _FirstPersonControls = FindObjectOfType<FirstPersonControls>();
        _ColorChangerScript = FindObjectOfType<ColorChangerScript>();
        //_RedBlinkingLights = FindObjectOfType<RedBlinkingLights>();
        Alarm = FindObjectOfType<Alarm>();
        _SoundScript = FindObjectOfType<SoundScript>();
        // counts the number of children in from the puzzleScreen object which is the parent
        int numPuzzles = puzzleScreen.transform.childCount;

        //initialize with the number of puzzles on the screen
        arrAllTiles = new GameObject[numPuzzles-1];
        
        //initialize with the number of correct puzzles on the screen
        arrAllCorrectTiles = new GameObject[numPuzzles-8];
        
        for (int i = 0; i < numPuzzles -1 ; i++)
        {
            // stores all the puzzles in the 
            arrAllTiles[i] = puzzleScreen.transform.GetChild(i).gameObject;
        }
        
        //counter used as an index for the array arrAllCorrectTiles
        int counter = 0;
        
        //Transverse through all the gameobjects in the arrAllTiles
        foreach (GameObject tile in arrAllTiles )
        {
            //if the pipes name is Cpipes it is a correct Tiles
            if (tile.name == "CTiles")
            {
                //we know store all correct tile in allCorrectTiles
                arrAllCorrectTiles[counter] = tile;
               // arrAllCorrectTilesPos[counter] = tile.transform;
                counter++;
                Debug.Log(tile.name);
            }
        }
        
        PuzzleRandomizer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //function that checks if the player completed the puzzle
    public bool IsPuzzleComplete()
    {
        //Transverses through the arrALlCorrectTiles array 
        foreach (GameObject tile in arrAllCorrectTiles)
        {
            if (tile.gameObject.CompareTag("StraightTiles"))
            {   
                //stores the rotation of the tile in a Vector 3 variable 
                var transformRotation = tile.transform.eulerAngles;
           
                //checks if the rotation of the tile is in its original position which is the correct format of the tile
                if (transformRotation.z != 0f && transformRotation.z != 180f && transformRotation.z != -180f)
                {
                    //puzzle continues since not all the correct tiles are in the correct positions
                    return false;
                }
            }
            else if (tile.gameObject.CompareTag("OtherTiles"))
            {
                //stores the rotation of the tile in a Vector 3 variable 
                var transformRotation = tile.transform.eulerAngles;
           
                //checks if the rotation of the tile is in its original position which is the correct format of the tile
                if (transformRotation.z != 0f)
                {
                    //puzzle continues since not all the correct tiles are in the correct positions
                    return false;
                }
            }
           
        }
        //puzzle ends since all the correct tiles are in the correct positions
        return true;
    }

    //function that randomizes the tiles rotation position
    private void PuzzleRandomizer()
    {
     
        //Transverses through the arrAllTiles array which contains the all the tile GameObjects
        foreach (GameObject tile in arrAllTiles)
        {
            //random number to randomize the rotation position of the tiles 
            int randomNumber = UnityEngine.Random.Range(0, 5);
            
            //storing the tiles rotation position in a Vector 3 variable
            var transformRotation = tile.transform.eulerAngles;

            //switch case statement that takes a random number then uses that number to choose what angle it will be rotated by
            switch (randomNumber)
            {
                case 0 : transformRotation.z -= 0f;
                    break;
                case 1 : transformRotation.z -= 90f;
                    break;
                case 2 : transformRotation.z -= 180f;
                    break;
                case 3 : transformRotation.z -= 270f;
                    break;
            }

            //sets the random rotation position of the tile to the current tile in the foreach loop
            tile.transform.eulerAngles = transformRotation;
        }
    }

    //function that opens the doors
    public void DoorOpener()
    {
        openDoor[0].SetActive(false);
        openDoor[1].SetActive(false);
        openDoor[2].SetActive(false);
        openDoor[3].SetActive(true);
        /*openDoor[3].transform.position = openDoor[0].transform.position;
        openDoor[2].transform.position = new Vector3(10000, 0, 0);*/
        openDoor[4].SetActive(true);
        openDoor[5].SetActive(false);

        DoorTrigger.SetActive(false);
        
    }
    
        //function that that robot uses to interact with interactible objects ( Code soon to be changed because of other interactable objects)
    public void IntertactWithObject()
    {
        //checks if the puzzle is not complete to continue
        if (!IsPuzzleComplete())
        {
            // Perform a raycast from the camera's position forward
            Ray ray = new Ray(_FirstPersonControls.playerCamera.position, _FirstPersonControls.playerCamera.forward);
            RaycastHit hit;
            
            // Debugging: Draw the ray in the Scene view
            Debug.DrawRay(_FirstPersonControls.playerCamera.position, _FirstPersonControls.playerCamera.forward * _FirstPersonControls.pickUpRange, Color.red, 2f);
            
            
            if (Physics.Raycast(ray, out hit, _FirstPersonControls.pickUpRange * 2))
            {
                //checks if the raycast hits the objects with the tags shown below
                if (hit.collider.CompareTag("Interactable") || hit.collider.CompareTag("OtherTiles") ||
                    hit.collider.CompareTag("StraightTiles"))
                {
                    //spot stores the transform of the hit that the raycast hit
                    Transform spot = hit.collider.transform;
                    
                    //we now make the position of the player to the position of the spot
                    _FirstPersonControls.Player.transform.position = spot.position;

                    //we now also make the rotation of the player to the rotation of the spot
                    var angles = _FirstPersonControls.Player.transform.eulerAngles;
                    angles.y = 90f;
                    _FirstPersonControls.Player.transform.eulerAngles = angles;
                    
                    //we know do assign the data of the robot to stop so it can focus on the object in hand
                    _FirstPersonControls.moveSpeed = 0;
                    _FirstPersonControls.jumpHeight = 0;
                    _FirstPersonControls.lookSpeed = 0;
                }
            }
        }
        else//if the player solved the puzzle do this
        {
            //function makes the player stop interacting with the object
            StopInteracting();
            PuzzleSolved = true;
            //function opens the doors after solving the puzzle
            //_AnimationScript.PlayBothAnimations();
            DoorOpener();
            _ColorChangerScript.MeshRenderer.materials[0].color = Color.green;
            
            //_RedBlinkingLights.StopBlinking();
            Alarm.StopAlarm();
        }
        
    }
    
    //this function allows the player interact with tiles in the puzzle
    public void InteractWithPuzzle()
    {
        //checks if the puzzle is already solved or not
        if (!IsPuzzleComplete())
        {
            //makes a raycase from the baby camera where the mouse is pointing
            Ray ray = new Ray(_FirstPersonControls.playerCamera.position, _FirstPersonControls.playerCamera.forward);
            RaycastHit hit;
            
            // Debugging: Draw the ray in the Scene view
            Debug.DrawRay(_FirstPersonControls.playerCamera.position, _FirstPersonControls.playerCamera.forward * _FirstPersonControls.pickUpRange, Color.red, 2f);

            
            if (Physics.Raycast(ray, out hit, _FirstPersonControls.pickUpRange*2))
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
        else //if the player solves the player they can move now
        {
            _FirstPersonControls.moveSpeed = _FirstPersonControls.tempSpeed;
            _FirstPersonControls.jumpHeight = _FirstPersonControls.tempJumpHeight;
            _FirstPersonControls.lookSpeed = _FirstPersonControls.tempLookAroundSpeed;
            
            // function opens the door
            DoorOpener();
            
            //makes the puzzle background green to show the puzzle is complete
           // _ColorChangerScript.MeshRenderer.materials[0].color = Color.green;
            _SoundScript.PlayAccessGrantedSound();
            _SoundScript.PlayBackgroundMusic();
            _SoundScript.StopAlarmSound();
                
           //_RedBlinkingLights.StopBlinking();
            Alarm.StopAlarm();
        }
    }

    //function that allows the player to start moving again
    public void StopInteracting()
    {
        _FirstPersonControls.moveSpeed = _FirstPersonControls.tempSpeed;
        _FirstPersonControls.jumpHeight = _FirstPersonControls.tempJumpHeight;
        _FirstPersonControls.lookSpeed = _FirstPersonControls.tempLookAroundSpeed;
    }
}
