using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    // Start is called before the first frame update
    void Start()
    {
        _BigRobotController = FindObjectOfType<BigRobotController>();
        _FirstPersonControls = FindAnyObjectByType<FirstPersonControls>();
        uiText.enabled = false; // Initially hide the text
        _HealthScript = FindObjectOfType<HealthScript>();
    }

    // Update is called once per frame
    void Update()
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
    
    // Handles Big Robot's raycast
    private void BigRobotUIRayCast()
    {
        Ray ray = new Ray(_BigRobotController.playerCamera.position, _BigRobotController.playerCamera.forward);
        RaycastHit hit;

        Debug.DrawRay(_BigRobotController.playerCamera.position,
            _BigRobotController.playerCamera.forward * _BigRobotController.pickUpRange, Color.red, _FirstPersonControls.pickUpRange);

        if (Physics.Raycast(ray, out hit, _BigRobotController.pickUpRange / 5))
        {
            if (hit.collider.CompareTag("Door"))
            {
                textTyper(hit , "Locked");
            }
            else if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("Chemicals") || hit.collider.CompareTag("Clue") || hit.collider.CompareTag("TestTube") ||
                     hit.collider.CompareTag("CanBePicked") || hit.collider.CompareTag("Switch") || hit.collider.CompareTag("Switch2"))
            {
               textTyper(hit ,"Press E / Sqaure to interact ");
            }
            else if (hit.collider.CompareTag("Robot"))
            {
                textTyper(hit , "Press Tab / L1 anytime to change to Wisp");
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
                     hit.collider.CompareTag("CanBePicked"))
            {
                textTyper(hit ,"Press E / Sqaure to interact ");
            }
            else if (hit.collider.CompareTag("Robot"))// plauer tag has problems
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
    
    //Fix raycast Ui
    //Make cutscenes
    //Pause menu
    //Dead screan
    //Torch for robots
}
