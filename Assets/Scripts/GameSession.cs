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

    public int GetBoxesInColumn(int indexPosition)
    {
        return boxesInColumn[indexPosition];
    }

    public void CountBoxesInColumns()
    {
        Square[] boxes = FindObjectsOfType<Square>();
        float xCoordinate = 0.5f;
        for (int i = 0; i < boxesInColumn.Length; i++)
        {
            boxesInColumn[i] = 0;
            foreach (Square box in boxes)
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
