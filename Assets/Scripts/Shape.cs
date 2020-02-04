using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool canMove = true;
    public int widthFromPivot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && canMove){
            canMove = false;
            float currentXPosition = transform.position.x;
            float newXPosition = Mathf.Clamp(currentXPosition + 0.5f, 0.5f, 5f - ((float)(widthFromPivot - 1) * 0.5f));
            Vector3 newPosition = new Vector3(newXPosition, transform.position.y, transform.position.z);
            transform.position = newPosition;
            canMove = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && canMove){
            canMove = false;
            float currentXPosition = transform.position.x;
            float newXPosition = Mathf.Clamp(currentXPosition - 0.5f, 0.5f, 5f - ((float)(widthFromPivot - 1) * 0.5f));
            Vector3 newPosition = new Vector3(newXPosition, transform.position.y, transform.position.z);
            transform.position = newPosition;
            canMove = true;
        }
       
    }
}
