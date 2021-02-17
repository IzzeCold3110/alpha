using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    private Terrain terrain;
    
    private TerrainData terrainData;

    private void Awake()
    {
        terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }

    private void Start()
    {
        EditTerrain();
    }

    private void EditTerrain()
    {
        int heightmapWidth = 10; // terrainData.heightmapResolution;
        int heightmapHeight = 10; // terrainData.heightmapResolution;

        float[,] heights = terrainData.GetHeights(50, 50, heightmapWidth, heightmapHeight);

        for(int z = 0; z < heightmapHeight; z++)
        {
            for (int x = 0; x < heightmapWidth; x++)
            {
                float cos = Mathf.Cos(x);
                float sin = -Mathf.Sin(z);
                heights[x, z] = (cos - sin) / 250;
            }
        }

        terrainData.SetHeights(50, 50, heights);
    }
}
