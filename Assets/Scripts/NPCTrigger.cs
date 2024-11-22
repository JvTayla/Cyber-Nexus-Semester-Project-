using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{
    
    public ParticleSystem [] particleSystems;

  
    public float particleDuration = 10f;


    NPCAnimator npcanimator;
    private UIScript _UIScript;
    private BigRobotController _BigRobotController;
    public NPCTalking _NPCTalking;
    private int i = 0;
    
    private void Start()
    { 
        npcanimator = FindObjectOfType<NPCAnimator>(); // This finds the first NPCAnimator in the scene
        _NPCTalking = FindObjectOfType<NPCTalking>();
        _BigRobotController = FindAnyObjectByType<BigRobotController>();

        if (particleSystems != null)
        {
            foreach (var ps in particleSystems)
            {
                if (ps != null)
                {
                    ps.Stop();
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Robot")) //Mightneedtochangeif we have Diff Tags for Robots
        {
            StartCoroutine(PlayParticleSystems());
            StartCoroutine(_NPCTalking.FirstYap());
            npcanimator.StartInteraction();
      
            // _BigRobotController.playerInput.Player.Disable();
        }

    }
    private IEnumerator PlayParticleSystems()
    {
       
        // Play all particle systems
        foreach (var ps in particleSystems)
        {
            if (ps != null)
            {
                ps.Play();
            }
        }

        // Wait for the duration
        yield return new WaitForSeconds(particleDuration);

        // Stop all particle systems
        foreach (var ps in particleSystems)
        {
            if (ps != null)
            {
                ps.Stop();
            }
        }
    }
}
