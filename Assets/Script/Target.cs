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

    void Start()
    {
        startPosition = transform.position;
        
        movementType = MovementType.Horizontal;
        
        if (minYPosition == 0f)
        {
            minYPosition = startPosition.y;
        }
        
        // Debug.Log($"Target started at {startPosition}, Movement Type: {movementType}, Speed: {movementSpeed}, Range: {movementRange}, Min Y: {minYPosition}");
    }

    void Update()
    {
        float deltaMovement = movementDirection * movementSpeed * Time.deltaTime;
        currentOffset += deltaMovement;
        
        if (movementType == MovementType.Horizontal)
        {
            // Horizontal
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
            // Vertical 
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
