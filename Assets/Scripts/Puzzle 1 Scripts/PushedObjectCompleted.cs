using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PushedObjectCompleted : MonoBehaviour
{
    public GameObject[] deactivateDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
           
            Vector3 centerPosition = transform.position; // Find Center of the CurrentGameObject ()
            other.transform.position = centerPosition; // Snap the pushable object to centre of gameObject

            // Deactivate doors
            foreach (GameObject obj in deactivateDoor)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}

