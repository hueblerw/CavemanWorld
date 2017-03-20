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
    public Texture2D[] splatTextures = new Texture2D[3];
    public PhysicMaterial colliderPhysics;
    public GameObject decidiousTree;


    // The method which initially builds the world view.
    public TerrainWorldView(World theWorld)
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
    private void PaintRocks(TerrainData terrainData)
    {

    }


    // Add the trees
    private void LoadTreePrototypes(TerrainData terrainData)
    {
        TreePrototype[] treeProtos = new TreePrototype[1];
        treeProtos[0] = new TreePrototype();
        treeProtos[0].prefab = decidiousTree;
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
        terrainData.size = new Vector3(X * 5, 4 * 5, Z * 5);
        HMWidth = terrainData.heightmapWidth;
        HMHeight = terrainData.heightmapHeight;
        Debug.Log(HMWidth + ", " + HMHeight);
        Debug.Log("scale: " + terrainData.heightmapScale);
        // Get and set the heights from the elevation map
        float[,] heights = terrainData.GetHeights(0, 0, HMWidth, HMHeight);
        // heights = currentWorld.
        terrainData.SetHeights(0, 0, heights);
        // Paint the rockinesss

        // Create the trees

    }


}
