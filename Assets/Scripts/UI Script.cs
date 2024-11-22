using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.IO;


public class UIScript : MonoBehaviour
{
    private FirstPersonControls _FirstPersonControls;
    private BigRobotController _BigRobotController;

    public TextMeshProUGUI uiText;
    private string currentText;
    private const float typingSpeed = 0.03f;
    private Coroutine typingCoroutine;
    private string lastDetectedObjectName = ""; // Track the last detected object name

    private HealthScript _HealthScript;

    private string Subtitles = "";
    private string Subtitles2 = "";
    private string Subtitles3 = "";
    private string Subtitles4 = "";

    private bool NpcInteract1 = false;
    private bool NpcInteract2 = false;

    public bool LoadingDeck;
    public bool Tag;
    public bool NuclearBattery;
    public bool[] Recordings = new bool[6];
    public bool Upload;
    public int tasksdone = 0;
    public TextMeshProUGUI MissionText;
    // Start is called before the first frame update
    void Start()
    {
        _BigRobotController = FindObjectOfType<BigRobotController>();
        _FirstPersonControls = FindAnyObjectByType<FirstPersonControls>();
        uiText.enabled = false; // Initially hide the text
        _HealthScript = FindObjectOfType<HealthScript>();
        LoadSubtitles();
        LoadSubtitles2();
        LoadSubtitles3();
        LoadSubtitles4();
        DisplayNextLine4();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NpcInteract1)
        {  
            if (_HealthScript.IsBigRobotInControl)
            {
                BigRobotUIRayCast(); 
            }
            else
            {
                SmallRobotUIRayCast(); 
            }
        }

        
        MissionText.text = Subtitles4;
        
        if (Input.GetKeyDown(KeyCode.Return)) // Checks if the Enter key is pressed
        {
            DisplayNextLine();
        }
        
    }
    
    // Handles Big Robot's raycast
    private void BigRobotUIRayCast()
    {
        Ray ray = new Ray(_BigRobotController.playerCamera.position, _BigRobotController.playerCamera.forward);
        Ray Jumpray = new Ray(_BigRobotController.playerCamera.position, -_BigRobotController.playerCamera.up); 
        RaycastHit hit;

        Debug.DrawRay(_BigRobotController.playerCamera.position,
            _BigRobotController.playerCamera.forward * _BigRobotController.pickUpRange, Color.red, _FirstPersonControls.pickUpRange);

        if (Physics.Raycast(ray, out hit, _BigRobotController.pickUpRange / 2) || Physics.Raycast(Jumpray, out hit, _BigRobotController.pickUpRange))
        {
            /*if (hit.collider.CompareTag("Door"))
            {
                textTyper(hit , "Locked");
            }*/
            if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("Chemicals") || hit.collider.CompareTag("Clue") || hit.collider.CompareTag("TestTube") ||
                     hit.collider.CompareTag("CanBePicked") || hit.collider.CompareTag("VoiceRecorder"))
                
            {
               textTyper(hit ,"Press E / Square to Pickup ");
            }
            else if(hit.collider.CompareTag("Switch") || hit.collider.CompareTag("Switch2"))
            {
                textTyper(hit ,"Press F / Square to Use ");
            }
            else if (hit.collider.CompareTag("BabyRobot"))
            {
                textTyper(hit , "Press Tab / L1 anytime to change to Wisp");
            }
            else  if (hit.collider.CompareTag("NPC") )
            {

                if (tasksdone < 1)
                {
                    MissionTasks();
                    tasksdone = 1;
                    NuclearBattery = false;
                    _BigRobotController.Allrecordings = false;
                }

             
                // _BigRobotController.NpcInteract = true;
            } 
            else if (hit.collider.CompareTag("NPC") && NuclearBattery)
            {
             
               
               //_BigRobotController.Battery = true;
            }
            else if (_BigRobotController.Recorderinhand)
            {
                textTyper(hit, Subtitles3);
            }
            else if (_BigRobotController.Allrecordings && hit.collider.CompareTag("NPC"))
            {
              // MissionTasks(); 
              

            }
            
            else if (hit.collider.CompareTag("Narrative"))
            {
                textTyper(hit, hit.collider.gameObject.name);
            }
            else if (hit.collider.CompareTag("Nuclear Battery"))
            {
                textTyper(hit, hit.collider.gameObject.name);
            }
            else if (hit.collider.CompareTag("SecurityTag"))
            {
                textTyper(hit, hit.collider.gameObject.name);
            }
            else
            {
                HideText();
            }
        }
        else
        {
            HideText();
        }

   

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot") && NuclearBattery)
        {
            if (tasksdone < 2)
            {
                MissionTasks();
            }
            tasksdone = 2;
        }
        
        if (other.CompareTag("Robot") && _BigRobotController.Allrecordings)
        {
            if (tasksdone < 3)
            {
                MissionTasks();
            }
            tasksdone = 3;
        }
    }
     
// Handles Small Robot's raycast
private void SmallRobotUIRayCast()
    {
        Ray ray = new Ray(_FirstPersonControls.playerCamera.position, _FirstPersonControls.playerCamera.forward);
        RaycastHit hit;

        Debug.DrawRay(_FirstPersonControls.playerCamera.position,
            _FirstPersonControls.playerCamera.forward * _FirstPersonControls.pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, _FirstPersonControls.pickUpRange / 5))
        {
            if (hit.collider.CompareTag("Door"))
            {
                textTyper(hit , "Locked");
            }
            else if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("Chemicals") || hit.collider.CompareTag("Clue") || hit.collider.CompareTag("TestTube") ||
                     hit.collider.CompareTag("CanBePicked") || hit.collider.CompareTag("VoiceRecorder"))
            {
                textTyper(hit ,"Press F / Square to interact ");
            }
            else if (hit.collider.CompareTag("BigRobot"))
            {
                textTyper(hit , "Press Tab / L1 anytime to change to Aurora");
            }
            else
            {
                HideText();
            }
        }
        else
        {
            HideText();
        }
    }

    // Coroutine to display text with a typing effect
    IEnumerator TypeText(string textToType)
    {
        uiText.text = ""; // Clear the text before typing
        uiText.enabled = true; // Make the text visible

        // Loop through each character in the string
        foreach (var t in textToType)
        {
            uiText.text += t; // Add one letter at a time

            // Force TMP to update
            uiText.ForceMeshUpdate(); // Ensure TMP text updates after each change

            yield return new WaitForSeconds(typingSpeed); // Wait for the typing speed before adding next letter
        }
    }

    private void textTyper(RaycastHit hit , string text)
    {
        string objectName = hit.collider.gameObject.name;

        // Check if the object detected is different from the last detected object
        if (objectName != lastDetectedObjectName)
        {
            lastDetectedObjectName = objectName; // Update the last detected object name

            // Stop any ongoing typing coroutine
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            // Start the typing effect with the new text
            typingCoroutine = StartCoroutine(TypeText(text));
        }
    }
    
    private void textTyperNoHit( string text)
    {
        //string objectName = hit.collider.gameObject.name;

        // Check if the object detected is different from the last detected object
        
            // Stop any ongoing typing coroutine
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            // Start the typing effect with the new text
            typingCoroutine = StartCoroutine(TypeText(text));
        
    }

    // Function to hide text and reset coroutine
    private void HideText()
    {
        uiText.enabled = false; // Hide the text
        lastDetectedObjectName = ""; // Reset the last detected object name
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop the typing coroutine if any
            typingCoroutine = null; // Reset the coroutine reference
        }
    }
    
    public string filePath = "Assets/Subtitles/Subtitles.txt"; // Path to your text file
    private List<string> subtitles = new List<string>();
    private int currentLineIndex = 0;

    void LoadSubtitles()
    {
        if (File.Exists(filePath))
        {
            subtitles.AddRange(File.ReadAllLines(filePath));
            Debug.Log("Subtitles loaded successfully.");
        }
        else
        {
            Debug.LogError("Subtitle file not found at " + filePath);
        }
    }

    void DisplayNextLine()
    {
        if (currentLineIndex < subtitles.Count)
        {
            Debug.Log(subtitles[currentLineIndex]); // Replace with your subtitle display logic (e.g., UI text element)
            Subtitles = subtitles[currentLineIndex];
            currentLineIndex++;
        }
        else
        {
            Debug.Log("End of subtitles.");
            _BigRobotController.NpcInteract = false;
        }
    }

    public void InteractWithNpc()
    {
        _BigRobotController.NpcInteract = true;
        HideText();
        DisplayNextLine();
        textTyperNoHit(Subtitles);
    }
    
    public string filePath2 = "Assets/Subtitles/Subtitles2.txt"; // Path to your text file
    private List<string> subtitles2 = new List<string>();
    private int currentLineIndex2 = 0;

    void LoadSubtitles2()
    {
        if (File.Exists(filePath2))
        {
            subtitles2.AddRange(File.ReadAllLines(filePath2));
            Debug.Log("Subtitles loaded successfully.");
        }
        else
        {
            Debug.LogError("Subtitle file not found at " + filePath2);
        }
    }

    void DisplayNextLine2()
    {
        if (currentLineIndex2 < subtitles2.Count)
        {
            Debug.Log(subtitles2[currentLineIndex2]); // Replace with your subtitle display logic (e.g., UI text element)
            Subtitles2 = subtitles2[currentLineIndex2];
            currentLineIndex2++;
        }
        else
        {
            Debug.Log("End of subtitles2.");
            _BigRobotController.NpcInteract = false;
        }
    }

    public void InteractWithNpc2(RaycastHit hit)
    {
        _BigRobotController.NpcInteract = true;
        HideText();
        DisplayNextLine2();
        textTyperNoHit(Subtitles2);
    }
    
    public string filePath3 = "Assets/Subtitles/Recordings.txt"; // Path to your text file
    private List<string> subtitles3 = new List<string>();
    private int currentLineIndex3 = 0;

    void LoadSubtitles3()
    {
        if (File.Exists(filePath3))
        {
            subtitles3.AddRange(File.ReadAllLines(filePath3));
            Debug.Log("Subtitles loaded successfully.");
        }
        else
        {
            Debug.LogError("Subtitle file not found at " + filePath3);
        }
    }

    void DisplayNextLine3()
    {
        if (currentLineIndex3 < subtitles3.Count)
        {
            Debug.Log(subtitles3[currentLineIndex3]); // Replace with your subtitle display logic (e.g., UI text element)
            Subtitles3 = subtitles3[currentLineIndex3];
            currentLineIndex3++;
        }
        else
        {
            Debug.Log("End of subtitles2.");
            _BigRobotController.NpcInteract = false;
        }
    }

    public void CollectRecording(RaycastHit hit)
    {
        _BigRobotController.NpcInteract = true;
        HideText();
        DisplayNextLine3();
        textTyperNoHit(Subtitles3);
        uiText.text = Subtitles3; 
    }
    
    public string filePath4 = "Assets/Subtitles/MissionTasks.txt"; // Path to your text file
    private List<string> subtitles4 = new List<string>();
    private int currentLineIndex4 = 0;

    void LoadSubtitles4()
    {
        if (File.Exists(filePath4))
        {
            subtitles4.AddRange(File.ReadAllLines(filePath4));
            Debug.Log("Subtitles loaded successfully.");
        }
        else
        {
            Debug.LogError("Subtitle file not found at " + filePath4);
        }
    }

   public void DisplayNextLine4()
    {
        if (currentLineIndex4 < subtitles4.Count)
        {
            Debug.Log(subtitles4[currentLineIndex4]); // Replace with your subtitle display logic (e.g., UI text element)
            Subtitles4 = subtitles4[currentLineIndex4];
            currentLineIndex4++;
        }
        else
        {
            Debug.Log("End of subtitles2.");
            _BigRobotController.NpcInteract = false;
        }
    }

    public void MissionTasks()
    {
       
        DisplayNextLine4();
        //text for missions
    }
    
   /* LoadingDeck;
    public bool Tag;
    public bool NuclearBattery;
    public bool Recording1;
    public bool Recording2;
    public bool Recording3;
    public bool Recording4;
    public bool Recording5;
    public bool Recording6;
    public bool Upload;*/
    //Fix raycast Ui
    //Make cutscenes
    //Pause menu
    //Dead screan
    //Torch for robots
}
