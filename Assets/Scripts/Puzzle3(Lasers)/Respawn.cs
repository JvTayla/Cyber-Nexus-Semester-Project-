using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform bigRobotRespawnPoint;
    public Transform smallRobotRespawnPoint;

    // Method to respawn a robot by name
    public void RespawnPlayer(string playerName)
    {
        // Find the player by name
        GameObject player = GameObject.Find(playerName);

        if (player != null)
        {
            Transform respawnPoint;

            // Determine which respawn point to use based on the player's name
            if (playerName == "Big Robot")
            {
                respawnPoint = bigRobotRespawnPoint;
            }
            else if (playerName == "Baby Robot ")
            {
                respawnPoint = smallRobotRespawnPoint;
            }
            else
            {
                Debug.LogWarning("No respawn point found for: " + playerName);
                return;
            }

            // Respawn the player at the appropriate respawn point
            player.transform.position = respawnPoint.position;
            player.transform.rotation = respawnPoint.rotation;

            Debug.Log(playerName + " has been respawned.");
        }
        else
        {
            Debug.LogError("Player not found: " + playerName);
        }
    }
}
