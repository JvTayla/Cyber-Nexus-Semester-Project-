using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraAnimationScript : MonoBehaviour
{
    public Transform BigRobotCamera;  // Big Robot camera's transform
    public Transform SmallRobotCamera; // Small Robot camera's transform
    public float transitionDuration = 5f; // Duration of the transition

    private bool isSwitching = false;
    private float minDistanceThreshold = 0.001f; // Minimum distance to consider camera at target position
    private float minRotationThreshold = 0.001f; // Minimum rotation difference to consider camera at target rotation

    // Call this method to switch to SmallRobotCamera
    public void SwitchToSmallRobot()
    {
        if (!isSwitching)
        {
            StartCoroutine(SwitchCamerasCoroutine(BigRobotCamera, SmallRobotCamera));
        }
    }

    // Call this method to switch back to BigRobotCamera
    public void SwitchToBigRobot()
    {
        if (!isSwitching)
        {
            StartCoroutine(SwitchCamerasCoroutine(SmallRobotCamera, BigRobotCamera));
        }
    }

    // Coroutine to switch from one camera to another with smooth transition
    private IEnumerator SwitchCamerasCoroutine(Transform fromCamera, Transform toCamera)
    {
        isSwitching = true;

        // Get initial positions and rotations
        Vector3 startPos = fromCamera.position;
        Quaternion startRot = fromCamera.rotation;

        Vector3 targetPos = toCamera.position;
        Quaternion targetRot = toCamera.rotation;

        // Interpolate over time
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;

            // Smoothly interpolate position and rotation
            fromCamera.position = Vector3.Lerp(startPos, targetPos, t);
            fromCamera.rotation = Quaternion.Slerp(startRot, targetRot, t);

            // Check if the camera has reached the destination within the thresholds
            if (Vector3.Distance(fromCamera.position, targetPos) <= minDistanceThreshold &&
                Quaternion.Angle(fromCamera.rotation, targetRot) <= minRotationThreshold)
            {
                break; // Stop the coroutine early if destination is reached
            }

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position and rotation are set
        fromCamera.position = targetPos;
        fromCamera.rotation = targetRot;

        isSwitching = false;
    }
}
