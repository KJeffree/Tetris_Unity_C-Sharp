using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] int[] boxesInColumn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int[] GetBoxesInColumn()
    {
        return boxesInColumn;
    }

    public void CountBoxesInColumns()
    {
        Box[] boxes = FindObjectsOfType<Box>();
        float xCoordinate = 0.5f;
        for (int i = 0; i < boxesInColumn.Length; i++)
        {
            boxesInColumn[i] = 0;
            foreach (Box box in boxes)
            {
                if (box.transform.position.x == xCoordinate)
                {
                    boxesInColumn[i] += 1;
                }
            }
            xCoordinate += 0.5f;
        }
    }
}
