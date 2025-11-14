using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float maxRadius = 0.6f;
    private float spawnTime;

    public enum MovementType
    {
        Horizontal,
        Vertical
    }

    [Header("Movement Settings")]
    public MovementType movementType = MovementType.Horizontal;
    public float movementRange = 5f;
    public float movementSpeed = 2f;
    public float minYPosition = 0f;

    private Vector3 startPosition;
    private float movementDirection = 1f;
    private float currentOffset = 0f;

    void Start()
    {
        spawnTime = Time.time;
        startPosition = transform.position;

        if (minYPosition == 0f)
        {
            minYPosition = startPosition.y;
        }
    }

    public void OnHit(Vector3 hitPoint)
    {
        Collider targetCollider = GetComponent<Collider>();
        float distance = Vector3.Distance(hitPoint, targetCollider.bounds.center);
        float accuracy = Mathf.Clamp01(1 - (distance / maxRadius));

        int score = Mathf.RoundToInt(accuracy * 100);

        float reactionTime = Time.time - spawnTime;
        float timeBonus = Mathf.Max(0, 1.5f - reactionTime);
        score += Mathf.RoundToInt(timeBonus * 20);

        GameManager.Instance.AddScore(score);
        Destroy(gameObject);
    }

    void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            return;
        }
        
        float deltaMovement = movementDirection * movementSpeed * Time.deltaTime;
        currentOffset += deltaMovement;

        if (movementType == MovementType.Horizontal)
        {
            float newZ = startPosition.z + currentOffset;

            if (currentOffset <= -movementRange)
            {
                movementDirection = 1f;
                currentOffset = -movementRange;
                newZ = startPosition.z + currentOffset;
            }
            else if (currentOffset >= movementRange)
            {
                movementDirection = -1f;
                currentOffset = movementRange;
                newZ = startPosition.z + currentOffset;
            }

            transform.position = new Vector3(startPosition.x, startPosition.y, newZ);
        }
        else
        {
            float newY = startPosition.y + currentOffset;

            if (newY <= minYPosition)
            {
                movementDirection = 1f;
                currentOffset = minYPosition - startPosition.y;
                newY = minYPosition;
            }
            else if (currentOffset >= movementRange)
            {
                movementDirection = -1f;
                currentOffset = movementRange;
                newY = startPosition.y + currentOffset;
            }

            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }
}
