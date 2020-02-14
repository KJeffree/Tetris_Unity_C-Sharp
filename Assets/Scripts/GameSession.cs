using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    float shapeSpawnXCoordinate = 2.75f;
    float shapeSpawnYCoordinate = 8.25f;

    [SerializeField] Shape[] availableShapes;
    [SerializeField] int[][] positionsOfSquares = new int[10][];
    // Start is called before the first frame update
    void Start()
    {
        CountPositionsOfSquares();
        SpawnNewShape();
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int[] GetPositionsOfSquares(int indexPosition)
    {
        return positionsOfSquares[indexPosition];
    }

    public void CountPositionsOfSquares()
    {
        Square[] squares = FindObjectsOfType<Square>();
        float xCoordinateOffset = 0.5f;
        float yCoordinateOffset = 0.5f;
        for(int i = 0; i < positionsOfSquares.Length; i++)
        {
            positionsOfSquares[i] = new int[18];
            
        }
        foreach (Square square in squares)
        {
            int xIndexPosition = (int)((square.GetXCoordinate() - xCoordinateOffset) / 0.5f);
            int yIndexPosition = (int)((square.GetYCoordinate() - yCoordinateOffset) / 0.5f);
            Debug.Log("x coordinate: " + square.GetXCoordinate());
            Debug.Log("y coordinate: " + square.GetYCoordinate());
            positionsOfSquares[xIndexPosition][yIndexPosition] = 1;
        }

        Debug.Log(positionsOfSquares[0][0]);
        Debug.Log(positionsOfSquares[1][0]);
        Debug.Log(positionsOfSquares[2][0]);
        Debug.Log(positionsOfSquares[3][0]);
        Debug.Log(positionsOfSquares[4][0]);
        Debug.Log(positionsOfSquares[5][0]);
        Debug.Log(positionsOfSquares[6][0]);
        Debug.Log(positionsOfSquares[7][0]);
        Debug.Log(positionsOfSquares[8][0]);
        Debug.Log(positionsOfSquares[9][0]);

    }

    public Shape GetRandomShape()
    {
        int index = Random.Range(0, availableShapes.Length);
        return availableShapes[index];
    }

    public void SpawnNewShape()
    {
        Vector2 spawnPoint = new Vector2(shapeSpawnXCoordinate, shapeSpawnYCoordinate);
        Instantiate(GetRandomShape(), spawnPoint, Quaternion.identity);
    }

    public int GetStatusOfPositionInGame(int xIndex, int yIndex)
    {
        if (xIndex < 0 || yIndex < 0)
        {
            return -1;
        }
        return positionsOfSquares[xIndex][yIndex];
    }
}
