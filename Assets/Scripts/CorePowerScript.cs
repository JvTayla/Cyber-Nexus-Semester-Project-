using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePowerScript : MonoBehaviour
{
    public GameObject bigRobotDeadScreen;
    public GameObject SmallRobotDeadScreen;
    public GameObject BigRobotUI;
    public GameObject SmallRobotUI;
    public GameObject bigRobotWarningObject;  // Warning UI or object for the Big Robot
    public GameObject smallRobotWarningObject;
    public float blinkInterval = 0.5f; // Time in seconds between each blink

       private bool isBigRobotBlinking = false;
    private bool isSmallRobotBlinking = false;

    public bool bigRobotWarningStage = false;
    public bool smallRobotWarningStage = false;

    public bool BigRobotDead;
    public bool SmallRobotDead;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Function for Big Robot Core Warning
    public void BigRobotCoreWarning()
    {
        if (!isBigRobotBlinking && bigRobotWarningObject != null)
        {
            isBigRobotBlinking = true;
            StartCoroutine(BlinkObject(bigRobotWarningObject, true));  // Start blinking for Big Robot
        }
    }

    // Function for Small Robot Core Warning
    public void SmallRobotCoreWarning()
    {
        if (!isSmallRobotBlinking && smallRobotWarningObject != null)
        {
            isSmallRobotBlinking = true;
            StartCoroutine(BlinkObject(smallRobotWarningObject, false));  // Start blinking for Small Robot
        }
    }

    // Coroutine to handle blinking for the robots
    IEnumerator BlinkObject(GameObject targetObject, bool isBigRobot)
    {
        while ((isBigRobot && isBigRobotBlinking) || (!isBigRobot && isSmallRobotBlinking))
        {
            // Toggle the object's active state to make it blink
            targetObject.SetActive(!targetObject.activeSelf);
            
            // Wait for the specified interval before blinking again
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    // Function to stop Big Robot warning
    public void StopBigRobotWarning()
    {
        isBigRobotBlinking = false;
        if (bigRobotWarningObject != null) bigRobotWarningObject.SetActive(false); // Ensure warning is off
    }

    // Function to stop Small Robot warning
    public void StopSmallRobotWarning()
    {
        isSmallRobotBlinking = false;
        if (smallRobotWarningObject != null) smallRobotWarningObject.SetActive(false); // Ensure warning is off
    }

    public void BigRobotShowDeadScreen()
    {
        bigRobotDeadScreen.SetActive(true);
        BigRobotUI.SetActive(false);
    }
    
    public void BigRobotHideDeadScreen()
    {
        bigRobotDeadScreen.SetActive(false);
       // BigRobotUI.SetActive(true);
    }
    
    public void SmallRobotShowDeadScreen()
    {
        SmallRobotDeadScreen.SetActive(true);
        SmallRobotUI.SetActive(false);
    }
    
    public void SmallRobotHideDeadScreen()
    {
        SmallRobotDeadScreen.SetActive(false);
       // SmallRobotUI.SetActive(true);
    }
}
