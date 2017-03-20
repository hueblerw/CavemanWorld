using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainWorldView : MonoBehaviour {

    // Variables
    private int X;
    private int Z;
    private int HMWidth;
    private int HMHeight;
    private World currentWorld;
    private float[] minMaxElevationValues;
    private float maxVertDist;
    public PhysicMaterial colliderPhysics;
    public GameObject[] treeModels;
    public Texture2D[] splatTextures = new Texture2D[3];

    // Constants
    private const int SQUARE_MULTIPLIER = 20 * 5; // Tiles on square side - 5 meters??? (20 feet???) for each square
    private const float HEIGHT_TO_WIDTH_RATIO = 0.2f;

    // The method which initially builds the world view.
    public void PassInTheWorld(World theWorld)
    {
        X = theWorld.WorldX;
        Z = theWorld.WorldZ;
        currentWorld = theWorld;
        CreateTerrainObjects();
    }


    // Based on video by quil18 at https://www.youtube.com/watch?v=U9-1Gr5CLgk
    private void CreateTerrainObjects()
    {
        // Create the empty obects that are needed.
        GameObject myself = GameObject.Find("TerrainTileMap");
        Terrain terrain = myself.AddComponent<Terrain>();
        TerrainCollider tCollide = myself.AddComponent<TerrainCollider>();
        TerrainData terrainData = new TerrainData();
        // Load some materials
        tCollide.material = colliderPhysics;
        BuildSplats(terrainData);
        LoadTreePrototypes(terrainData);
        // Apply the data in here
        ApplyModel(terrainData);
        PaintRocks(terrainData);
        // Connect the terrain data to the terrain object
        terrain.terrainData = terrainData;
        tCollide.terrainData = terrainData;
    }


    // Paint the rocks texture in appropriately
    // Maybe also add rock detail thingies
    private void PaintRocks(TerrainData battlefieldData)
    {

    }


    // Add the trees
    private void LoadTreePrototypes(TerrainData terrainData)
    {
        TreePrototype[] treeProtos = new TreePrototype[3];
        for (int i = 0; i < splatTextures.Length; i++)
        {
            treeProtos[i] = new TreePrototype();
            treeProtos[i].prefab = treeModels[i];
        }
        terrainData.treePrototypes = treeProtos;
    }


    // Populates the textures onto their respective objects
    private void BuildSplats(TerrainData terrainData)
    {
        SplatPrototype[] splatProtos = new SplatPrototype[splatTextures.Length];
        for (int i = 0; i < splatTextures.Length; i++)
        {
            splatProtos[i] = new SplatPrototype();
            splatProtos[i].texture = splatTextures[i];
        }
        terrainData.splatPrototypes = splatProtos;
    }


    private void ApplyModel(TerrainData terrainData)
    {
        terrainData.heightmapResolution = X + 1;
        terrainData.baseMapResolution = Z + 1;
        terrainData.SetDetailResolution(64, 32);
        // Set the size after the resoultion always
        minMaxElevationValues = currentWorld.elevationVertices.getMinMaxValues();
        maxVertDist = minMaxElevationValues[1] - minMaxElevationValues[0];
        terrainData.size = new Vector3(X * SQUARE_MULTIPLIER, maxVertDist * HEIGHT_TO_WIDTH_RATIO * SQUARE_MULTIPLIER, Z * SQUARE_MULTIPLIER);
        HMWidth = terrainData.heightmapWidth;
        HMHeight = terrainData.heightmapHeight;
        Debug.Log(HMWidth + ", " + HMHeight);
        // Debug.Log("scale: " + terrainData.heightmapScale);
        // Get and set the heights for hills and rockiness
        float[,] heights = terrainData.GetHeights(0, 0, HMWidth, HMHeight);
        heights = ConvertElevationToHeights(heights);
        terrainData.SetHeights(0, 0, heights);
        // Create the trees
    }


    private float[,] ConvertElevationToHeights(float[,] heights)
    {
        for (int i = 0; i < HMWidth; i++)
        {
            for (int j = 0; j < HMHeight; j++)
            {
                // Debug.Log((int)((i / (double) HMWidth) * X) + ", " + (int)((j / (double)HMWidth) * X));
                heights[i, j] = (currentWorld.elevationVertices.worldArray[(int)((i / (double)HMWidth) * X), (int)((j / (double)HMHeight) * Z)] - minMaxElevationValues[0]) / maxVertDist;
            }
        }
        return heights;
    }

}
