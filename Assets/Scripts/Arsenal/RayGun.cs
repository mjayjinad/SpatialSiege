using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayGun : MonoBehaviour
{
    [SerializeField] private LineRenderer linePrefab;
    [SerializeField] private GameObject rayImpactPrefab;
    [SerializeField] private List<Transform> shootingPoints;  // List of shooting positions
    [SerializeField] private float maxLineDistance = 5;
    [SerializeField] private float lineShowTimer = 0.3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private LayerMask layermask;

    private bool isShooting = false; // Track if shooting is currently happening

    public void Shoot()
    {
        // Only allow shooting if not already shooting
        if (isShooting) return;

        StartCoroutine(ShootWithDelay());
    }

    private IEnumerator ShootWithDelay()
    {
        // Set shooting flag to true
        isShooting = true;

        audioSource.PlayOneShot(audioClip);

        // Loop through each shooting point
        foreach (Transform shootingPoint in shootingPoints)
        {
            Vector3 endPoint = Vector3.zero;
            Ray ray = new Ray(shootingPoint.position, shootingPoint.forward);

            bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, layermask);

            if (hasHit)
            {
                endPoint = hit.point;

                // Check if the hit object is the player
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerHealthManager playerHealth = hit.collider.GetComponent<PlayerHealthManager>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(0.5f);
                    }
                }

                Quaternion rayImpactRotation = Quaternion.LookRotation(-hit.normal);
                GameObject rayImpact = Instantiate(rayImpactPrefab, hit.point, rayImpactRotation);
                Destroy(rayImpact, 0.3f);
            }
            else
            {
                endPoint = shootingPoint.position + shootingPoint.forward * maxLineDistance;
            }

            // Create the line renderer
            LineRenderer line = Instantiate(linePrefab);
            line.positionCount = 2;
            line.SetPosition(0, shootingPoint.position);
            line.SetPosition(1, endPoint);

            // Destroy the line renderer after some time
            Destroy(line.gameObject, lineShowTimer);
        }

        // Wait for the line to be destroyed before shooting again
        yield return new WaitForSeconds(lineShowTimer * 3);

        // Set shooting flag back to false after shooting is done
        isShooting = false;
    }
}
