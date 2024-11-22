using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTasks : MonoBehaviour
{
    private UIScript _UIScript;

    private BigRobotController _BigRobotController;
    // Start is called before the first frame update
    void Start()
    {
        _UIScript = FindAnyObjectByType<UIScript>();
        _BigRobotController = FindAnyObjectByType<BigRobotController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot") && ( _UIScript.NuclearBattery || _BigRobotController.Battery || _BigRobotController.HasNuclearBattery) )
        {
            if (_UIScript.tasksdone < 2)
            {
                _UIScript.MissionTasks();
            }
            _UIScript.tasksdone = 2;
        }
        
        if (other.CompareTag("Robot") && _BigRobotController.Allrecordings)
        {
            if (_UIScript.tasksdone < 3)
            {
                _UIScript.MissionTasks();
            }
            _UIScript.tasksdone = 3;
        }
    }
}
