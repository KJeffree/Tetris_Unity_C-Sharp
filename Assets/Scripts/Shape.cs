﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] int[] widthsFromPivot;
    [SerializeField] int[] widthsBeforePivot;
    [SerializeField] int[] widthBelowPivot;

    bool stopMovement = false;

    int rotation = 0;

    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginMovement());
        gameSession = FindObjectOfType<GameSession>();
    }

    IEnumerator BeginMovement()
    {
        yield return new WaitForSeconds(1);
        InvokeRepeating("ShapeMovementDown", 0, 1f);
    }

    private void ShapeMovementDown()
    {
        float currentYPosition = transform.position.y;
        int overlap = CalculateAnyOverlapWithAnotherShape(0.0f, -0.5f);
        float newYPosition = currentYPosition - 0.5f + (overlap * 0.5f);
        MoveShape(transform.position.x, newYPosition, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(StopMovement());
        stopMovement = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        stopMovement = false;
    }

    IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(0.9f);
        if (stopMovement)
        {
            Destroy(GetComponent<Shape>());
            gameSession.CountPositionsOfSquares();
            gameSession.SpawnNewShape();
        }
        stopMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)){
            float xDifference = Input.GetKeyDown(KeyCode.RightArrow) ? 0.5f : -0.5f;
            int overlap = CalculateAnyOverlapWithAnotherShape(xDifference, 0.0f);
            float currentXPosition = transform.position.x;
            float newXPosition = clampXPositionWithinGameSpace(currentXPosition + xDifference);
            MoveShape(newXPosition - (overlap * xDifference), transform.position.y, transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateShape();
            float xPosition = clampXPositionWithinGameSpace(transform.position.x);
            float yPosition = transform.position.y;
            MoveShape(xPosition, yPosition, transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveShapeFullyDown();
        }
    }

    private void MoveShapeFullyDown()
    {
        float yDifference = -0.5f;
        bool looping = true;
        while (looping)
        {
            int overlap = CalculateAnyOverlapWithAnotherShape(0.0f, yDifference);
            // Debug.Log("y difference start of loop: " + yDifference);
            // Debug.Log(overlap);
            if (overlap == 0 && (transform.position.y + yDifference - (0.5f * widthBelowPivot[rotation])) == 0.75f)
            {
                looping = false;
            }
            if (overlap != 0)
            {
                looping = false;
                yDifference += 0.5f;
            } else {
                yDifference -= 0.5f;
            }

        }
        // Debug.Log("y difference end of loop: " + yDifference);

        MoveShape(transform.position.x, transform.position.y + yDifference, transform.position.z);
        CancelInvoke("ShapeMovementDown");
        // gameSession.CountPositionsOfSquares();
    }



    private float clampXPositionWithinGameSpace(float xPosition)
    {
        return Mathf.Clamp(xPosition, 0.25f + ((float)(widthsBeforePivot[rotation]) * 0.5f), 4.75f - ((float)(widthsFromPivot[rotation] - 1) * 0.5f));
    }

    private void RotateShape()
    {
        Vector3 originalRotation = transform.rotation.eulerAngles;
        originalRotation.z -= 90;
        transform.rotation = Quaternion.Euler(originalRotation);
        rotation = rotation < 3 ? rotation + 1 : 0;
        int overlap = CalculateAnyOverlapWithAnotherShape();
        MoveShape(transform.position.x, transform.position.y + (overlap * 0.5f), transform.position.z);
        AlterShapeCollider();
    }

    private int CalculateAnyOverlapWithAnotherShape(float xPositionAlteration = 0.0f, float yPositionAlteration = 0.0f)
    {
        Square[] squares = GetComponentsInChildren<Square>();
        int[] numberOfOverlapsPerColumn = new int[squares.Length];
        int largestOverlap = 0;
        for (int i = 0; i < squares.Length; i++)
        {
            int xIndexPosition = (int)((squares[i].GetXCoordinate() - 0.5f + xPositionAlteration) / 0.5f);
            int yIndexPosition = (int)((squares[i].GetYCoordinate() - 0.5f + yPositionAlteration) / 0.5f);
            // Debug.Log(xIndexPosition);
            // Debug.Log(yIndexPosition);
            int squareStatus = gameSession.GetStatusOfPositionInGame(xIndexPosition, yIndexPosition);
            // Debug.Log(squareStatus);
            if (squareStatus == 1)
            {
                numberOfOverlapsPerColumn[i] += 1;
                if (numberOfOverlapsPerColumn[i] > largestOverlap)
                {
                    largestOverlap = numberOfOverlapsPerColumn[i];
                }
            }
        }
        return largestOverlap;
    }

    private void AlterShapeCollider()
    {
        foreach (Square child in GetComponentsInChildren<Square>())
        {
            child.alterCollider(rotation);
        }
    }

    private void MoveShape(float xPosition, float yPosition, float zPosition)
    {
        Vector3 newPosition = new Vector3(xPosition, yPosition, zPosition);
        transform.position = newPosition;
    }
}
