using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject targetPrefab;
    public float totalSpawnTime = 30f; 
    
    private float gameTimer = 0f;
    private bool isSpawning = false;
    private GameObject currentTarget = null;
    private GameObject prefabReference = null; 

    void Start()
    {
        if (targetPrefab != null)
        {
            prefabReference = targetPrefab;
        }
        else
        {
            Debug.LogError("Target Prefab is not assigned in TargetSpawner!");
            return;
        }
        
        isSpawning = false;
        gameTimer = 0f;
        
        FindAndTrackOriginalTarget();
    }

    void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            return;
        }
        
        if (!isSpawning && GameManager.Instance.GameStarted)
        {
            isSpawning = true;
            gameTimer = 0f;
        }
        
        if (!isSpawning) return;

        gameTimer += Time.deltaTime;
        
        if (gameTimer >= totalSpawnTime)
        {
            isSpawning = false;
            return;
        }

        if (currentTarget == null || currentTarget == false)
        {
            SpawnTarget();
        }
    }

    void FindAndTrackOriginalTarget()
    {
        Target[] allTargets = FindObjectsOfType<Target>();
        
        foreach (Target target in allTargets)
        {
            if (!target.name.Contains("(Clone)"))
            {
                currentTarget = target.gameObject;
                break;
            }
        }
    }

    void SpawnTarget()
    {
        GameObject prefabToUse = prefabReference != null ? prefabReference : targetPrefab;
        
        if (prefabToUse == null)
        {
            Debug.LogError("Target Prefab is not available! Make sure to assign the Prefab Asset (from Assets/Prefab/Target.prefab), not the scene instance.");
            return;
        }

        if (currentTarget != null && currentTarget != false)
        {
            return;
        }

        Vector3 spawnPosition = GetSpawnPosition();
        
        currentTarget = Instantiate(prefabToUse, spawnPosition, Quaternion.identity);
        
        Target targetScript = currentTarget.GetComponent<Target>();
        if (targetScript == null)
        {
            targetScript = currentTarget.AddComponent<Target>();
            
            Target originalTarget = FindOriginalTargetForSettings();
            if (originalTarget != null)
            {
                targetScript.movementRange = originalTarget.movementRange;
                targetScript.movementSpeed = originalTarget.movementSpeed;
                targetScript.minYPosition = originalTarget.minYPosition;
                targetScript.maxRadius = originalTarget.maxRadius;
            }
        }
        
        targetScript.movementType = (Target.MovementType)Random.Range(0, 2);
        
        TagAllColliders(currentTarget);
    }

    void TagAllColliders(GameObject target)
    {
        Collider[] allColliders = target.GetComponentsInChildren<Collider>();
        
        foreach (Collider col in allColliders)
        {
            col.gameObject.tag = "Target";
        }
        
        target.tag = "Target";
    }

    Target FindOriginalTargetForSettings()
    {
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
        float x = 80f;
        float y = -24f;
        float z = Random.Range(110f, 120f);
        
        return new Vector3(x, y, z);
    }
}
