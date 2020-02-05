using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] int[] widthsFromPivot;
    [SerializeField] int[] widthsBeforePivot;

    bool stopMovement = false;

    int rotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BeginMovement());   
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)){
            float xDifference = Input.GetKeyDown(KeyCode.RightArrow) ? 0.5f : -0.5f;
            float currentXPosition = transform.position.x;
            float newXPosition = clampXPositionWithinGameSpace(currentXPosition + xDifference);
            Vector3 newPosition = new Vector3(newXPosition, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            RotateShape();
            float xPosition = clampXPositionWithinGameSpace(transform.position.x);
            Vector3 newPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }

    private float clampXPositionWithinGameSpace(float xPosition)
    {
        return Mathf.Clamp(xPosition, 0.5f + ((float)(widthsBeforePivot[rotation]) * 0.5f), 5f - ((float)(widthsFromPivot[rotation] - 1) * 0.5f));
    }

    private void RotateShape()
    {
        Vector3 originalRotation = transform.rotation.eulerAngles;
        originalRotation.z -= 90;
        transform.rotation = Quaternion.Euler(originalRotation);
        rotation = rotation < 3 ? rotation + 1 : 0;
        AlterShapeCollider();
    }

    private void AlterShapeCollider()
    {
        foreach (Box child in GetComponentsInChildren<Box>())
        {
            child.alterCollider(rotation);
        }
    }
}
