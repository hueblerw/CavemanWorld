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
    public PhysicMaterial colliderPhysics;
    public GameObject[] treeModels;
    public Texture2D[] splatTextures = new Texture2D[3];

    // Constants
    private const int SQUARE_MULTIPLIER = 20 * 5; // Tiles on square side - 5 meters??? (20 feet???) for each square


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
        terrainData.size = new Vector3(X * SQUARE_MULTIPLIER, 4 * SQUARE_MULTIPLIER, Z * SQUARE_MULTIPLIER);
        HMWidth = terrainData.heightmapWidth;
        HMHeight = terrainData.heightmapHeight;
        Debug.Log(HMWidth + ", " + HMHeight);
        // Debug.Log("scale: " + terrainData.heightmapScale);
        // Get and set the heights for hills and rockiness
        float[,] heights = terrainData.GetHeights(0, 0, HMWidth, HMHeight);
        // heights = current.CreateHeightsArray(HMWidth, HMHeight);
        terrainData.SetHeights(0, 0, heights);
        // Create the trees
    }


}
