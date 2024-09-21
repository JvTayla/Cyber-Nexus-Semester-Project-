using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    
    public Transform player;
    public Transform respawnPoint;

    public void RespawnPlayer()
    {
        
        player.position = respawnPoint.position;
        player.rotation = respawnPoint.rotation;
        
    }

    
}
