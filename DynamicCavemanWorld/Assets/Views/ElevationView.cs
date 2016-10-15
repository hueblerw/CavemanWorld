﻿using UnityEngine;
using System.Collections;

public class ElevationView : MonoBehaviour {

    public static Mesh BuildMesh(SingleValueLayer elevationVerticesLayer)
    {
        // Set some constants
        float[,] elevations = elevationVerticesLayer.worldArray;
        int numOfTilesX = SingleValueLayer.WORLDX;
        int numOfTilesZ = SingleValueLayer.WORLDZ;
        int numOfTiles = numOfTilesX * numOfTilesZ;
        int numVertices = (numOfTilesX + 1) * (numOfTilesZ + 1);
        float tileSize = 5.0f;
        float heightScale = 1.0f;
        
        // Convert to mesh data
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[2 * numOfTiles * 3];
        Vector3[] normals = new Vector3[numVertices];

        // Create the vertices and the normals generically
        int x, z;
        int squareIndex, triOffset;
        for (z = 0; z < numOfTilesZ + 1; z++)
        {
            for (x = 0; x < numOfTilesX + 1; x++)
            {
                vertices[z * (numOfTilesX + 1) + x] = new Vector3(x * tileSize, elevations[x, z] * heightScale, z * tileSize);
                normals[z * (numOfTilesX + 1) + x] = Vector3.up;
                // uv[z * (numOfTilesX + 1) + x] = new Vector2((float) x / (numOfTilesX + 1), (float) z / (numOfTilesZ + 1));
            }
        }
        // Create the triangle generically
        for (z = 0; z < numOfTilesZ; z++)
        {
            for (x = 0; x < numOfTilesX; x++)
            {
                squareIndex = z * numOfTilesX + x;
                triOffset = squareIndex * 6;
                // first triangle
                triangles[triOffset] = z * (numOfTilesX + 1) + x + 0;
                triangles[triOffset + 2] = z * (numOfTilesX + 1) + x + (numOfTilesX + 1) + 1;
                triangles[triOffset + 1] = z * (numOfTilesX + 1) + x + (numOfTilesX + 1) + 0; ;
                // second triangle
                triangles[triOffset + 3] = z * (numOfTilesX + 1) + x + 0;
                triangles[triOffset + 5] = z * (numOfTilesX + 1) + x + 1;
                triangles[triOffset + 4] = z * (numOfTilesX + 1) + x + (numOfTilesX + 1) + 1; ;
            }
        }

                // Create a new mesh and populate it with the data from the elevation layer
                Mesh world = new Mesh();
        world.vertices = vertices;
        world.triangles = triangles;
        world.normals = normals;

        // Return our mesh to the controller
        return world;
    }

    // Build the texture for the world
    public static Texture BuildTexture(int worldx, int worldz)
    {
        int pixelsPerTile = 1;
        Texture2D texture = new Texture2D(worldx * pixelsPerTile, worldz * pixelsPerTile);
        for (int x = 0; x < worldx * pixelsPerTile; x++)
        {
            for (int z = 0; z < worldz * pixelsPerTile; z++)
            {
                Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                texture.SetPixel(x, z, color);
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    } 

}
