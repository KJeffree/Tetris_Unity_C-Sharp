using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetXCoordinate(){
        // Debug.Log(transform.position.x);
        return transform.position.x;
    }

    public float GetYCoordinate(){
        return transform.position.y;
    }

    public void alterCollider(int rotation)
    {
        if (rotation % 2 == 0)
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = new Vector2(0.4f, 0.4384615f);
        } else {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = new Vector2(0.4923077f, 0.4f);
        }
        
    }
}
