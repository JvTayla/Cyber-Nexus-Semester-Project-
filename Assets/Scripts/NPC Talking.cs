using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalking : MonoBehaviour
{

    public BigRobotController bigRobotController;

    public GameObject RobotTriggerNuclearBattery;
    public GameObject RobotTriggerRecordingsFound;

    public GameObject YapText1;
    public GameObject YapText2;
    public GameObject YapText3;
    public GameObject YapText4;
    public GameObject YapText5;
    public GameObject YapText6;
    public GameObject YapText7;
    public GameObject YapText8;
    public GameObject YapText9;
    public GameObject YapText10;
    public GameObject YapText11;
    public GameObject YapText12;

    public GameObject BehindText;
    

    private void Awake()
    {
        YapText1.SetActive(false);
        YapText2.SetActive(false);
        YapText3.SetActive(false);
        YapText4.SetActive(false);
        YapText5.SetActive(false);
        YapText6.SetActive(false);
        YapText7.SetActive(false);
        YapText8.SetActive(false);
        YapText9.SetActive(false);
        YapText10.SetActive(false);
        YapText11.SetActive(false);
        YapText12.SetActive(false);
        BehindText.SetActive(false);

        RobotTriggerNuclearBattery.SetActive(false);
        RobotTriggerRecordingsFound.SetActive(false);

        // Find the BigRobotController script (assuming it's on a GameObject in the scene)
        bigRobotController = FindObjectOfType<BigRobotController>();

        // Check if the script was found
        if (bigRobotController == null)
        {
            Debug.LogError("BigRobotController not found in the scene!");
            return;
        }

    }


    public IEnumerator FirstYap()
    {
        BehindText.SetActive(true);
        YapText1.SetActive(true);
 
        yield return new WaitForSeconds(5f);

        YapText1.SetActive(false);
        YapText2.SetActive(true);

        yield return new WaitForSeconds(7f);

        YapText2.SetActive(false);
        YapText3.SetActive(true);
       
 
        yield return new WaitForSeconds(7f);

        YapText3.SetActive(false);
        YapText4.SetActive(true);

        yield return new WaitForSeconds(7f);

        YapText3.SetActive(false);
        YapText4.SetActive(true);

        yield return new WaitForSeconds(7f);

        YapText4.SetActive(false);
        YapText5.SetActive(true);

        yield return new WaitForSeconds(3f);

        YapText5.SetActive(false);
        YapText6.SetActive(true);

        yield return new WaitForSeconds(7f);

        YapText6.SetActive(false);
        YapText7.SetActive(true);

        yield return new WaitForSeconds(7f);

        //Set the rest of the canvases off for the next robot 
        DeactivateAllYapTexts();

        RobotTriggerNuclearBattery.SetActive(true);
    }


    public IEnumerator SecondYap()
    {
        if (bigRobotController.HasNuclearBattery)
        {
            BehindText.SetActive(true);
            YapText8.SetActive(true);

            yield return new WaitForSeconds(7f);

            YapText8.SetActive(false);
            YapText9.SetActive(true);

            yield return new WaitForSeconds(10f);

            YapText9.SetActive(false);
            YapText10.SetActive(true);

            yield return new WaitForSeconds(6f);

            //Set the rest of the canvases off for the next robot 
            DeactivateAllYapTexts();

            RobotTriggerNuclearBattery.SetActive(false);
            RobotTriggerRecordingsFound.SetActive(true);
        }
    }

    public IEnumerator ThirdYap()
    {
        if (bigRobotController.HasSecurityTag)
        {
            BehindText.SetActive(true);
            YapText11.SetActive(true);

            yield return new WaitForSeconds(7f);

            YapText11.SetActive(false);
            YapText12.SetActive(true);

            yield return new WaitForSeconds(7f);

            //Set the rest of the canvases off for the next robot 
            DeactivateAllYapTexts();
            RobotTriggerRecordingsFound.SetActive(false);
        }
    }

    private void DeactivateAllYapTexts()
    {
        YapText1.SetActive(false);
        YapText2.SetActive(false);
        YapText3.SetActive(false);
        YapText4.SetActive(false);
        YapText5.SetActive(false);
        YapText6.SetActive(false);
        YapText7.SetActive(false);
        YapText8.SetActive(false);
        YapText9.SetActive(false);
        YapText10.SetActive(false);
        YapText11.SetActive(false);
        YapText12.SetActive(false);
        BehindText.SetActive(false);

    }





}
