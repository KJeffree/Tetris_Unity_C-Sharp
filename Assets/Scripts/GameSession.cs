using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class GameSession : MonoBehaviour
{

    float shapeSpawnXCoordinate = 2.75f;
    float shapeSpawnYCoordinate = 8.25f;

    public int[] pointsForNumberOfLines = new int[4];

    public int score = 0;

    public float shapeSpeed = 1f;

    public int numberOfFullLinesCreated = 0;

    int level = 0;

    Shape nextShape;

    [SerializeField] GameObject nextShapeImage;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Shape[] availableShapes;
    [SerializeField] List<Square[]> positionsOfSquares = new List<Square[]>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 18; i++)
        {
            positionsOfSquares.Add(new Square[10]);
        }
        SetNewNextShape();
        SpawnNewShape();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreText();
        UpdateLevelText();
    }

    public void UpdateNextShapeImage()
    {
        nextShapeImage.GetComponent<UnityEngine.UI.Image>().sprite = nextShape.GetSprite();
    }

    public float GetShapeSpeed()
    {
        return shapeSpeed;
    }

    public void IncreaseShapeSpeed()
    {
        shapeSpeed -= 0.05f;
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void IncreaseNumberOfLinesCreated()
    {
        numberOfFullLinesCreated++;
        if (numberOfFullLinesCreated % 8 == 0)
        {
            LevelUp();
        }
    }

    public void UpdateLevelText()
    {
        levelText.text = level.ToString();
    }

    public void AddPointsToScore(int points)
    {
        score += points;
    }

    public void LevelUp()
    {
        level++;
        IncreaseShapeSpeed();
    }

    public int CalculatePointsToAdd(int numberOfLinesCleared)
    {
        return pointsForNumberOfLines[numberOfLinesCleared - 1] * (level + 1);
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
            IncreaseNumberOfLinesCreated();
        }
        int counter = 0;
        foreach (int index in fullRowsIndex)
        {
            RemoveFullRows(index - counter);
            counter++;
        }
        if (fullRowsIndex.Count > 0)
        {
            int points = CalculatePointsToAdd(fullRowsIndex.Count);
            AddPointsToScore(points);
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
        bool sameShape = true;
        int index = 0;
        while(sameShape)
        {
            index = UnityEngine.Random.Range(0, availableShapes.Length);
            if (availableShapes[index] != nextShape)
            {
                sameShape = false;
            }
        }
        
        return availableShapes[index];
    }

    public void SetNewNextShape()
    {
        nextShape = GetRandomShape();
    }

    public void SpawnNewShape()
    {
        Vector2 spawnPoint = new Vector2(shapeSpawnXCoordinate, shapeSpawnYCoordinate);
        Instantiate(nextShape, spawnPoint, Quaternion.identity);
        SetNewNextShape();
        UpdateNextShapeImage();

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
