using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject targetPrefab;
    public Transform arenaTransform;  // Assign the Arena game object in the inspector
    public float spawnInterval = 2f;  // Time between spawns
    public float totalSpawnTime = 30f; // Total time to spawn targets
    
    [Header("Arena Bounds")]
    public Vector3 arenaCenter = Vector3.zero;
    public Vector3 arenaSize = new Vector3(50f, 20f, 50f); // Adjust based on your Arena size
    
    private float spawnTimer = 0f;
    private float gameTimer = 0f;
    private bool isSpawning = false;
    private GameObject currentTarget = null;

    void Start()
    {
        // If arenaTransform is not assigned, try to find it
        if (arenaTransform == null)
        {
            GameObject arena = GameObject.Find("Arena 1");
            if (arena != null)
            {
                arenaTransform = arena.transform;
                CalculateArenaBounds();
            }
        }
        else
        {
            CalculateArenaBounds();
        }
        
        StartSpawning();
    }

    void Update()
    {
        if (!isSpawning) return;

        gameTimer += Time.deltaTime;
        
        // Stop spawning after total spawn time
        if (gameTimer >= totalSpawnTime)
        {
            isSpawning = false;
            return;
        }

        // Check if current target is destroyed or doesn't exist
        if (currentTarget == null)
        {
            spawnTimer += Time.deltaTime;
            
            // Spawn new target after spawn interval
            if (spawnTimer >= spawnInterval)
            {
                SpawnTarget();
                spawnTimer = 0f;
            }
        }
    }

    void StartSpawning()
    {
        isSpawning = true;
        gameTimer = 0f;
        spawnTimer = 0f;
        SpawnTarget(); // Spawn first target immediately
    }

    void SpawnTarget()
    {
        if (targetPrefab == null)
        {
            Debug.LogError("Target Prefab is not assigned!");
            return;
        }

        // Calculate random position within arena bounds
        Vector3 randomPosition = GetRandomPositionInArena();
        
        // Spawn the target
        currentTarget = Instantiate(targetPrefab, randomPosition, Quaternion.identity);
    }

    Vector3 GetRandomPositionInArena()
    {
        // Calculate random position within arena bounds
        float x = Random.Range(arenaCenter.x - arenaSize.x / 2f, arenaCenter.x + arenaSize.x / 2f);
        float y = Random.Range(arenaCenter.y - arenaSize.y / 2f, arenaCenter.y + arenaSize.y / 2f);
        float z = Random.Range(arenaCenter.z - arenaSize.z / 2f, arenaCenter.z + arenaSize.z / 2f);
        
        return new Vector3(x, y, z);
    }

    void CalculateArenaBounds()
    {
        if (arenaTransform == null) return;

        // Get all renderers in the arena to calculate bounds
        Renderer[] renderers = arenaTransform.GetComponentsInChildren<Renderer>();
        
        if (renderers.Length > 0)
        {
            Bounds bounds = renderers[0].bounds;
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            
            arenaCenter = bounds.center;
            arenaSize = bounds.size;
        }
        else
        {
            // Fallback: use arena transform position and default size
            arenaCenter = arenaTransform.position;
            // You may need to adjust these values based on your Arena size
            arenaSize = new Vector3(50f, 20f, 50f);
        }
    }
}
