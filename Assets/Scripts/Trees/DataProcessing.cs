using System;
using System.Collections.Generic;
using UnityEngine;

public static class DataProcessing
{
    public static List<Vector2Int> directions = new List<Vector2Int>() { 
        new Vector2Int( 0, 1), //N
        new Vector2Int( 1, 1), //NE
        new Vector2Int( 1, 0), //E
        new Vector2Int(-1, 1), //SE
        new Vector2Int(-1, 0), //S
        new Vector2Int(-1,-1), //SW
        new Vector2Int( 0,-1), //W
        new Vector2Int( 1,-1)  //NW
    };

    public static List<Vector2Int> FindLocalMaxima(float[,] dataMatrix, int xCoord, int zCoord)
    {
        List<Vector2Int> maximas = new List<Vector2Int>();
        for (int x = 0; x < dataMatrix.GetLength(0); x++)
        {
            for (int z = 0; z < dataMatrix.GetLength(1); z++)
            {
                var noiseVal = dataMatrix[x, z];

                // Checks that neighbouring tiles are less than this tile with position:(x,z)
                if (CheckNeighbours(dataMatrix, x, z, (neighbourNoise) => neighbourNoise < noiseVal)) 
                {
                    maximas.Add(new Vector2Int(xCoord + x, zCoord + z));
                }
            }
        }
        return maximas;
    }

    private static bool CheckNeighbours(float[,] dataMatrix, int x, int y, Func<float, bool> successCondition)
    {
        foreach (var dir in directions) 
        {
            var newPost = new Vector2Int(x + dir.x, y + dir.y);

            if(newPost.x < 0 || newPost.x >= dataMatrix.GetLength(0) || newPost.y < 0 || newPost.y >= dataMatrix.GetLength(1))
            {
                continue;
            }
            if(successCondition(dataMatrix[x + dir.x, y + dir.y]) == false) 
            {
                return false;
            }
        
        }
        return true;
    }
}