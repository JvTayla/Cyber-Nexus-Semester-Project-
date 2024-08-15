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
    private Transform[] arrAllTilesPos;//array that will store all the tiles transform positions
    private GameObject[] arrAllCorrectTiles; // array that will store all the correct pipe tiles gameobjects
    private Transform[] arrAllCorrectTilesPos; //array that will store all the corrects pipes tile gameobjects
    // Start is called before the first frame update
    void Start()
    {
        //Self plagrism NB
        
        // counts the number of children in from the puzzleScreen object which is the parent
        int numPuzzles = puzzleScreen.transform.childCount;

        //initialize with the number of puzzles on the screen
        arrAllTiles = new GameObject[numPuzzles-1];
        
        //initialize with the number of correct puzzles on the screen
        arrAllCorrectTiles = new GameObject[numPuzzles-9];
        
        for (int i = 0; i < numPuzzles -1 ; i++)
        {
            // stores all the puzzles in the 
            arrAllTiles[i] = puzzleScreen.transform.GetChild(i).gameObject;
            Debug.Log(arrAllTiles[i].name);
        }
        
        //Transverse through all the gameobjects in the arrAllTiles
        foreach (GameObject tile in arrAllTiles )
        {
            int i = 0; 
            //if the pipes name is Cpipes it is a correct Tiles
            if (tile.name == "CPipes")
            {
                //we know store all correct tile in allCorrectTiles
                arrAllCorrectTiles[i] = tile;
            }
        }
        
        PuzzleRandomizer();
    }

    // Update is called once per frame
    void Update()
    {
        ///if (IsPuzzleComplete())
        //{
        //    Debug.Log("Yes");
       // }
    }

    public bool IsPuzzleComplete()
    {
        //Transverses through the arrALlCorrectTiles array 
        foreach (GameObject tile in arrAllCorrectTiles)
        {
            //checks if the rotation of the tile is in its original position which is the correct format of the tile
            if (tile.transform.rotation.z != 0f)
            {
                return false;
            }
        }
        return true;
    }

    private void PuzzleRandomizer()
    {
        
        foreach (GameObject tile in arrAllTiles)
        {
            int randomNumber = UnityEngine.Random.Range(0, 5);

            var transformRotation = tile.transform.eulerAngles;

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

            tile.transform.eulerAngles = transformRotation;

            var angles = tile.transform.eulerAngles;
            angles.z = Mathf.Repeat(transformRotation.z,360f);
            tile.transform.eulerAngles = angles;
        }
    }
}
