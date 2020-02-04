using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] int[] widthsFromPivot;
    [SerializeField] int[] widthsBeforePivot;

    int rotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)){
            float xDifference = Input.GetKeyDown(KeyCode.RightArrow) ? 0.5f : -0.5f;
            float currentXPosition = transform.position.x;
            float newXPosition = Mathf.Clamp(currentXPosition + xDifference, 0.5f + ((float)(widthsBeforePivot[rotation]) * 0.5f), 5f - ((float)(widthsFromPivot[rotation] - 1) * 0.5f));
            Vector3 newPosition = new Vector3(newXPosition, transform.position.y, transform.position.z);
            transform.position = newPosition;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow)){
            Vector3 originalRotation = transform.rotation.eulerAngles;
            originalRotation.z -= 90;
            transform.rotation = Quaternion.Euler(originalRotation);
            rotation = rotation < 3 ? rotation + 1 : 0;
        }
    }
}
