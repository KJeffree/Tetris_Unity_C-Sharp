using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] int[] widthsFromPivot;
    [SerializeField] int[] widthsBeforePivot;
    [SerializeField] int[] widthBelowPivot;

    bool stopMovement = false;
    bool colliding = false;

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
        float newYPosition = currentYPosition - 0.5f;
        Vector3 newPosition = new Vector3(transform.position.x, newYPosition, transform.position.z);
        transform.position = newPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(StopMovement());
        stopMovement = true;
        colliding = true;
        gameSession.CountBoxesInColumns();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        stopMovement = false;
        colliding = false;
    }

    IEnumerator StopMovement()
    {
        yield return new WaitForSeconds(0.9f);
        if (stopMovement)
        {
            Destroy(GetComponent<Shape>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)){
            float xDifference = Input.GetKeyDown(KeyCode.RightArrow) ? 0.5f : -0.5f;
            float currentXPosition = transform.position.x;
            float newXPosition = clampXPositionWithinGameSpace(currentXPosition + xDifference);
            MoveShape(newXPosition, transform.position.y, transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            RotateShape();
            float xPosition = clampXPositionWithinGameSpace(transform.position.x);
            float yPosition = transform.position.y;
            MoveShape(xPosition, yPosition, transform.position.z);
        }
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
        CalculateAnyOverlapWithAnotherShape();
        AlterShapeCollider();
    }

    private int CalculateAnyOverlapWithAnotherShape()
    {
        List<float> boxXCoordinatesOfShape = new List<float>();
        float shapeXCoordinate = transform.position.x;
        for (int i = 0; i < widthsBeforePivot[rotation]; i++)
        {
            boxXCoordinatesOfShape.Add(shapeXCoordinate - 0.5f * (i + 1));
        }
        for (int i = 0; i < widthsFromPivot[rotation]; i++)
        {
            boxXCoordinatesOfShape.Add(shapeXCoordinate + 0.5f * i);
        }
        Debug.Log(boxXCoordinatesOfShape.Count);
        float shapeLowestYCoordinate = transform.position.y - widthBelowPivot[rotation] * 0.5f;

        int largestOverlap = 0;

        foreach (float coordinate in boxXCoordinatesOfShape)
        {
            int indexOfCoordinate = (int)(coordinate / 0.5f);
            Debug.Log(indexOfCoordinate);

            float heightOfCurrentColumn = 0.5f + gameSession.GetBoxesInColumn()[indexOfCoordinate] * 0.5f;

            if (heightOfCurrentColumn > shapeLowestYCoordinate)
            {
                int overlap = (int)(heightOfCurrentColumn - shapeLowestYCoordinate / 0.5f);
                if (overlap > largestOverlap)
                {
                    largestOverlap = overlap;
                }
            }
        }
        Debug.Log(largestOverlap);
        return largestOverlap;
    }

    private void AlterShapeCollider()
    {
        foreach (Box child in GetComponentsInChildren<Box>())
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
