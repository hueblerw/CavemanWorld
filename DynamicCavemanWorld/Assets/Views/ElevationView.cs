﻿using UnityEngine;
using System.Collections;
using System;

// Based on code by quill18 in his video series linked to here: https://www.youtube.com/watch?v=bpB4BApnKhM
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
        Vector2[] uv = new Vector2[numVertices];

        // Create the vertices and the normals generically
        int x, z;
        int squareIndex, triOffset;
        for (z = 0; z < numOfTilesZ + 1; z++)
        {
            for (x = 0; x < numOfTilesX + 1; x++)
            {
                vertices[z * (numOfTilesX + 1) + x] = new Vector3(x * tileSize, elevations[x, z] * heightScale, z * tileSize);
                normals[z * (numOfTilesX + 1) + x] = Vector3.up;
                uv[z * (numOfTilesX + 1) + x] = new Vector2((float) x / (numOfTilesX + 1), (float) z / (numOfTilesZ + 1));
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
        world.uv = uv;

        // Return our mesh to the controller
        return world;
    }

    // Build the texture for the world
    public static Texture BuildTexture(World world)
    {
        // Initialize some variables
        int worldx = world.WorldX;
        int worldz = world.WorldZ;
        int pixelsPerTile = 10;
        int adjustedX;
        int adjustedZ;
        float[,] elevations = world.elevation.worldArray;
        float greenTint;
        float redTint;
        float blueTint;
        Color color;

        // Create a texture object
        Texture2D texture = new Texture2D(worldx * pixelsPerTile, worldz * pixelsPerTile);
        for (int x = 0; x < worldx * pixelsPerTile; x++)
        {
            for (int z = 0; z < worldz * pixelsPerTile; z++)
            {
                adjustedX = (int)Math.Truncate((double) x / pixelsPerTile);
                adjustedZ = (int)Math.Truncate((double) z / pixelsPerTile);
                // If underwater make it a shade of blue
                if (elevations[adjustedX, adjustedZ] < 0.0f)
                {
                    blueTint = 1f;
                    redTint = 0f;
                    greenTint = (100f + elevations[adjustedX, adjustedZ] * 10f) / 253f;
                    if (greenTint < 0)
                    {
                        greenTint = 0;
                    }
                }
                // else make it a shade of green/brown
                else 
                {
                    if(elevations[adjustedX, adjustedZ] < 20.0f)
                    {
                        if (elevations[adjustedX, adjustedZ] < 10f)
                        {
                            blueTint = 0f;
                            redTint = 0f;
                            greenTint = (53f + (10f - elevations[adjustedX, adjustedZ]) * 20f) / 253f;
                        }
                        else
                        {
                            blueTint = 0f;
                            greenTint = (103f - elevations[adjustedX, adjustedZ] * 5f) / 253f;
                            redTint = ((20f - elevations[adjustedX, adjustedZ]) * 5f) / 253f;
                        }
                    }
                    else
                    {
                        // Beyond 20 will be coded as white
                        redTint = 1f;
                        blueTint = 1f;
                        greenTint = 1f;
                    }
                }
                color = new Color(redTint, greenTint, blueTint);
                texture.SetPixel(x, z, color);
            }
        }

        // Apply the texture  and return it
        //texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    } 

}
