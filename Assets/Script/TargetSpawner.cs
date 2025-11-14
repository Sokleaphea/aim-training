using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject targetPrefab;
    public float totalSpawnTime = 30f; // Total time to spawn targets
    
    private float gameTimer = 0f;
    private bool isSpawning = false;
    private GameObject currentTarget = null;
    private GameObject prefabReference = null; // Backup reference to the prefab asset

    void Start()
    {
        // Store a backup reference to the prefab before anything gets destroyed
        // This ensures we can still spawn even if the original target is destroyed
        if (targetPrefab != null)
        {
            prefabReference = targetPrefab;
        }
        else
        {
            Debug.LogError("Target Prefab is not assigned in TargetSpawner!");
            return;
        }
        
        // Don't start spawning yet - wait for game to start
        isSpawning = false;
        gameTimer = 0f;
        
        // Find and track the original target in the scene
        FindAndTrackOriginalTarget();
    }

    void Update()
    {
        // Wait for game to start before spawning
        if (!GameManager.Instance.GameStarted)
        {
            return;
        }
        
        // Start spawning when game starts
        if (!isSpawning && GameManager.Instance.GameStarted)
        {
            isSpawning = true;
            gameTimer = 0f;
        }
        
        if (!isSpawning) return;

        gameTimer += Time.deltaTime;
        
        // Stop spawning after total spawn time
        if (gameTimer >= totalSpawnTime)
        {
            isSpawning = false;
            return;
        }

        // Check if current target is destroyed or doesn't exist
        // Use == false to properly check for destroyed Unity objects
        if (currentTarget == null || currentTarget == false)
        {
            // Spawn a new clone immediately when current target is destroyed
            SpawnTarget();
        }
    }

    void FindAndTrackOriginalTarget()
    {
        // Find the original Target object in the scene (not a clone)
        Target[] allTargets = FindObjectsOfType<Target>();
        
        foreach (Target target in allTargets)
        {
            // Track the original target (the one not spawned by us)
            if (!target.name.Contains("(Clone)"))
            {
                currentTarget = target.gameObject;
                break;
            }
        }
    }

    void SpawnTarget()
    {
        // Use the backup prefab reference if targetPrefab becomes null
        GameObject prefabToUse = prefabReference != null ? prefabReference : targetPrefab;
        
        if (prefabToUse == null)
        {
            Debug.LogError("Target Prefab is not available! Make sure to assign the Prefab Asset (from Assets/Prefab/Target.prefab), not the scene instance.");
            return;
        }

        // Only spawn if no target currently exists
        if (currentTarget != null && currentTarget != false)
        {
            return;
        }

        // Calculate spawn position: fixed x and y, random z
        Vector3 spawnPosition = GetSpawnPosition();
        
        // Spawn a clone of the target prefab and track it
        currentTarget = Instantiate(prefabToUse, spawnPosition, Quaternion.identity);
        
        // Ensure the Target script component exists (in case prefab doesn't have it)
        Target targetScript = currentTarget.GetComponent<Target>();
        if (targetScript == null)
        {
            // Add the Target script if it's missing
            targetScript = currentTarget.AddComponent<Target>();
            
            // Copy settings from the original target if it exists
            Target originalTarget = FindOriginalTargetForSettings();
            if (originalTarget != null)
            {
                targetScript.movementRange = originalTarget.movementRange;
                targetScript.movementSpeed = originalTarget.movementSpeed;
                targetScript.minYPosition = originalTarget.minYPosition;
                targetScript.maxRadius = originalTarget.maxRadius;
            }
        }
        
        // Randomly choose between Horizontal and Vertical movement
        targetScript.movementType = (Target.MovementType)Random.Range(0, 2);
        
        // Tag ALL colliders in the spawned target (including child objects)
        TagAllColliders(currentTarget);
    }

    void TagAllColliders(GameObject target)
    {
        // Get all colliders in the target and its children
        Collider[] allColliders = target.GetComponentsInChildren<Collider>();
        
        foreach (Collider col in allColliders)
        {
            // Set the "Target" tag on the GameObject that has the collider
            col.gameObject.tag = "Target";
        }
        
        // Also tag the root GameObject
        target.tag = "Target";
    }

    Target FindOriginalTargetForSettings()
    {
        // Find the original Target object to copy settings from
        Target[] allTargets = FindObjectsOfType<Target>();
        
        foreach (Target target in allTargets)
        {
            if (!target.name.Contains("(Clone)"))
            {
                return target;
            }
        }
        return null;
    }

    Vector3 GetSpawnPosition()
    {
        // Fixed x and y, random z in range 110-120
        float x = 80f;
        float y = -24f;
        float z = Random.Range(110f, 120f);
        
        return new Vector3(x, y, z);
    }
}
