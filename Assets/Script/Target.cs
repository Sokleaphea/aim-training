using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum MovementType
    { Horizontal, Vertical }

    [Header("Movement Settings")]
    public MovementType movementType = MovementType.Horizontal;
    public float movementRange = 5f;
    public float movementSpeed = 2f;
    public float minYPosition = 0f; 

    private Vector3 startPosition;
    private float movementDirection = 1f;
    private float currentOffset = 0f;
    private GameManager gameManager;

    void Start()
    {
        startPosition = transform.position;
        gameManager = GameManager.Instance;
        
        movementType = MovementType.Horizontal;
        
        if (minYPosition == 0f)
        {
            minYPosition = startPosition.y;
        }
    }

    void Update()
    {
        if (gameManager == null || !gameManager.IsGameStarted)
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
