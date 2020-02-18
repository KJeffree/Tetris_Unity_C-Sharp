using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameSession : MonoBehaviour
{

    float shapeSpawnXCoordinate = 2.75f;
    float shapeSpawnYCoordinate = 8.25f;

    [SerializeField] Shape[] availableShapes;
    [SerializeField] List<Square[]> positionsOfSquares = new List<Square[]>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 18; i++)
        {
            positionsOfSquares.Add(new Square[10]);
        }
        SpawnNewShape();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Square[] GetPositionsOfSquares(int indexPosition)
    {
        return positionsOfSquares[indexPosition];
    }

    public void CountPositionsOfSquares()
    {
        Square[] squares = FindObjectsOfType<Square>();
        float xCoordinateOffset = 0.5f;
        float yCoordinateOffset = 0.5f;
        foreach (Square square in squares)
        {
            int xIndexPosition = (int)Math.Round(((square.GetXCoordinate() - xCoordinateOffset) / 0.5f), 0);
            int yIndexPosition = (int)Math.Round(((square.GetYCoordinate() - yCoordinateOffset) / 0.5f), 0);
            positionsOfSquares[yIndexPosition][xIndexPosition] = square;
        }
        FindAndRemoveRowsIfFull();
    }

    public void FindAndRemoveRowsIfFull()
    {
        List<int> fullRowsIndex = FindFullRows();
        foreach (int index in fullRowsIndex)
        {
            MoveSquaresDownFromAboveIndex(index);
        }
        int counter = 0;
        foreach (int index in fullRowsIndex)
        {
            RemoveFullRows(index - counter);
            counter++;
        }
    }

    public List<int> FindFullRows()
    {
        List<int> fullRowIndexes = new List<int>();
        for (int i = 0; i < 18; i++)
        {
            bool full = true;
            foreach (Square square in positionsOfSquares[i])
            {
                if (square == null)
                {
                    full = false;
                }
            }
            if (full)
            {
                fullRowIndexes.Add(i);
            }
        }
        return fullRowIndexes;
    }

    public void RemoveFullRows(int index)
    {
        foreach(Square square in positionsOfSquares[index])
        {
            Destroy(square.gameObject);
        }
        positionsOfSquares.RemoveAt(index);
        positionsOfSquares.Add(new Square[10]);
    }

    public void MoveSquaresDownFromAboveIndex(int index)
    {
        for (int i = index + 1; i < positionsOfSquares.Count; i++)
        {
            foreach (Square square in positionsOfSquares[i])
            {
                if (square != null)
                {
                    square.transform.position = new Vector2(square.transform.position.x, square.transform.position.y - 0.5f);
                }
            }
        }
    }

    public Shape GetRandomShape()
    {
        int index = UnityEngine.Random.Range(0, availableShapes.Length);
        return availableShapes[index];
    }

    public void SpawnNewShape()
    {
        Vector2 spawnPoint = new Vector2(shapeSpawnXCoordinate, shapeSpawnYCoordinate);
        Instantiate(GetRandomShape(), spawnPoint, Quaternion.identity);
        // Instantiate(availableShapes[4], spawnPoint, Quaternion.identity);
    }

    public int GetStatusOfPositionInGame(int xIndex, int yIndex)
    {
        if (xIndex < 0 || yIndex < 0)
        {
            return -1;
        }
        return positionsOfSquares[yIndex][xIndex] == null ? 0 : 1;
    }
}
