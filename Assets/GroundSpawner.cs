using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject platformPrefab;
    public int initialPlatforms = 5;
    public Transform playerTransform;

    [Header("Spawn Control")]
    public float spawnThreshold = 20f; // Distance ahead of the player to spawn new platforms

    private float platformLength;
    private float nextSpawnZ;
    private Queue<GameObject> activePlatforms = new Queue<GameObject>();

    private void Start()
    {
        // Get platform length automatically
        if (platformPrefab != null)
        {
            MeshRenderer renderer = platformPrefab.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                platformLength = renderer.bounds.size.z;
            }
            else
            {
                Debug.LogError("Platform Prefab is missing a MeshRenderer component! Setting default length.");
                platformLength = 10f; // Default length if MeshRenderer is missing
            }
        }
        else
        {
            Debug.LogError("Platform Prefab is not assigned!");
            return;
        }

        // Initialize platforms
        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnPlatform();
        }
    }

    private void Update()
    {
        // Check if more platforms need to be spawned
        if (playerTransform.position.z + spawnThreshold > nextSpawnZ)
        {
            SpawnPlatform();
            DespawnPlatform();
        }
    }

    private void SpawnPlatform()
    {
        GameObject platform = Instantiate(platformPrefab, 
            new Vector3(0, 0, nextSpawnZ), Quaternion.identity, this.transform);
        activePlatforms.Enqueue(platform);
        nextSpawnZ += platformLength;
    }

    private void DespawnPlatform()
    {
        if (activePlatforms.Count > initialPlatforms)
        {
            GameObject oldPlatform = activePlatforms.Dequeue();
            Destroy(oldPlatform);
        }
    }
}
