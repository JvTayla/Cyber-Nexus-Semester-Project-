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
    // Start is called before the first frame update
    void Start()
    {
        //Self plagrism NB
        
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
        
        foreach (GameObject door in openDoor)
        {
            if (door != null)
            {
                //opens the doors by hiding them
                door.SetActive(false);
            }
        }
    }
}
