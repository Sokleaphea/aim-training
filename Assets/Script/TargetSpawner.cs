using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject targetPrefab;
    public Transform arenaTransform;  
    public float spawnInterval = 0.5f;  
    public float totalSpawnTime = 30f; 
    
    [Header("Arena Bounds")]
    public Vector3 arenaCenter = Vector3.zero;
    public Vector3 arenaSize = new Vector3(50f, 20f, 50f); 
    
    private float spawnTimer = 0f;
    private float gameTimer = 0f;
    private bool isSpawning = false;
    private GameObject currentTarget = null;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        
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
    }

    void Update()
    {
        if (gameManager == null || !gameManager.IsGameStarted)
        {
            return;
        }
        
        if (!isSpawning)
        {
            StartSpawning();
        }
        
        if (!isSpawning) return;

        gameTimer += Time.deltaTime;
        
        if (gameTimer >= totalSpawnTime)
        {
            isSpawning = false;
            return;
        }

        if (currentTarget == null)
        {
            spawnTimer += Time.deltaTime;

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
        SpawnTarget(); 
    }

    void SpawnTarget()
    {
        if (targetPrefab == null)
        {
            Debug.LogError("Target Prefab is not assigned!");
            return;
        }

        Vector3 randomPosition = GetRandomPositionInArena();
        
        currentTarget = Instantiate(targetPrefab, randomPosition, Quaternion.identity);
    }

    Vector3 GetRandomPositionInArena()
    {
        float x = Random.Range(arenaCenter.x - arenaSize.x / 2f, arenaCenter.x + arenaSize.x / 2f);
        float y = Random.Range(arenaCenter.y - arenaSize.y / 2f, arenaCenter.y + arenaSize.y / 2f);
        float z = Random.Range(arenaCenter.z - arenaSize.z / 2f, arenaCenter.z + arenaSize.z / 2f);
        
        return new Vector3(x, y, z);
    }

    void CalculateArenaBounds()
    {
        if (arenaTransform == null) return;

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
            arenaCenter = arenaTransform.position;
            arenaSize = new Vector3(50f, 20f, 50f);
        }
    }
}
