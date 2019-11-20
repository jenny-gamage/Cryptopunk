﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridGenerator 
{
    internal static int[][] GenerateGrid(int numSegments)
    {
        int[][] grid = new int[8][];
        for(int i=0;i<grid.Length;i++)
        {
            grid[i] = new int[8];
            for(int j=0;j<grid[i].Length;j++)
            {
                if(i==0||j==0)
                {
                    grid[i][j] = 1;
                }
                else
                { 
                    grid[i][j] = 0;
                }
            }
        }
        return grid;
    }
}
