using Meta.XR.MRUtilityKit;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrbSpawner : MonoBehaviour
{
    public List<Transform> spawnedOrbs;
    [SerializeField] private int numberOfOrbsToSpawn;
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float height;
    [SerializeField] private float minEdgeDistance;
    [SerializeField] private MRUKAnchor.SceneLabels spawnLabel;

    private int maxSapwnTryCount = 100;
    private int currentSpawnTryCount = 0;

    public static OrbSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //MRUK.Instance.RegisterSceneLoadedCallback(InitializeOrb);
        //InitializeOrb();
        GameEventManager.Instance.OnWaveEndedEvent += ReloadOrbsOnWaveEnded;
    }

    public void InitializeOrb()
    {
        StartCoroutine(DelayOrbSpawning());
    }

    private IEnumerator DelayOrbSpawning()
    {
        yield return new WaitForSeconds(1);

        SpawnOrbs();
    }

    private void SpawnOrbs()
    {
        for (int i = 0; i < numberOfOrbsToSpawn; i++)
        {
            Vector3 randomPosition = Vector3.zero;

            MRUKRoom room = MRUK.Instance.GetCurrentRoom();
            LabelFilter labelFilter = new LabelFilter(spawnLabel);

            bool positionFound = false;
            currentSpawnTryCount = 0;  // Reset the spawn attempt count for each orb spawn

            // Retry logic
            while (currentSpawnTryCount < maxSapwnTryCount)
            {
                bool hasFound = room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_UP, minEdgeDistance, labelFilter, out randomPosition, out Vector3 norm);
                
                if (hasFound && IsPositionOnNavMesh(randomPosition))
                {
                    positionFound = true;  // Valid position found
                    break;  // Exit the while loop if a valid position is found
                }

                currentSpawnTryCount++;
            }

            if (!positionFound)
            {
                Debug.LogWarning("Failed to find a valid position for orb after " + maxSapwnTryCount + " attempts.");
                continue;  // Skip spawning this orb and move to the next iteration
            }

            // Ensure the orb's y position is set correctly
            randomPosition.y = height;

            // Instantiate the orb at the valid NavMesh position
            GameObject go = Instantiate(orbPrefab, randomPosition, Quaternion.identity);
            spawnedOrbs.Add(go.transform);
        }
    }

    private bool IsPositionOnNavMesh(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 20.0f, NavMesh.AllAreas))  // Increased maxDistance
        {
            return true;
        }
        Debug.Log("Couldn't find a position");
        return false;
    }

    private void ReloadOrbsOnWaveEnded(Wave wave)
    {
        foreach (var orb in spawnedOrbs)
        {
            Destroy(orb.gameObject);
        }

        spawnedOrbs.Clear();
        SpawnOrbs();
    }

    public void DestroyOrb(GameObject orb)
    {
        spawnedOrbs.Remove(orb.transform);
        Destroy(orb);

        if(spawnedOrbs.Count == 0)
        {
            SpawnOrbs();
        }
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.OnWaveEndedEvent -= ReloadOrbsOnWaveEnded;
    }
}
