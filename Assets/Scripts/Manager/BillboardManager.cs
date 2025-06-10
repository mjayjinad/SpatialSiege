using UnityEngine;

public class BillboardManager : MonoBehaviour
{
    public float baseTiltAngle = 15f;  // Base tilt when the camera is level with the drone
    public float maxTiltAngle = 45f;   // Maximum tilt angle when the camera is very low (bending down)
    public float tiltDistanceFactor = 10f;  // A factor to adjust tilt based on camera height difference

    private void Update()
    {
        // Ensure that the enemy is always facing the camera
        Vector3 directionToPlayer = Camera.main.transform.position - transform.position;
        directionToPlayer.y = 0f;  // Keep the enemy from rotating on the y-axis (upward direction)

        if (directionToPlayer != Vector3.zero)  // Avoid divide by zero error
        {
            // Calculate the rotation to face the player (camera)
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);

            // Calculate the difference in height between the camera and the drone
            float heightDifference = Camera.main.transform.position.y - transform.position.y;

            // Dynamically adjust the tilt angle based on the height difference
            float dynamicTiltAngle = Mathf.Lerp(baseTiltAngle, maxTiltAngle, Mathf.Abs(heightDifference) / tiltDistanceFactor);

            // Apply the dynamic tilt to the x-axis (looking downward)
            rotation *= Quaternion.Euler(dynamicTiltAngle, 0f, 0f);  // Apply dynamic tilt

            // Apply the new rotation to the enemy
            transform.rotation = rotation;
        }
    }
}
