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
    private float[] treePositions;
    public PhysicMaterial colliderPhysics;
    public GameObject[] treeModels;
    public Texture2D[] splatTextures = new Texture2D[7];
    public GameObject ocean;

    // Constants
    public const int SQUARE_MULTIPLIER = 20 * 5; // Tiles on square side - 5 meters??? (20 feet???) for each square
    private const float HEIGHT_TO_WIDTH_RATIO = 0.2f;

    // The method which initially builds the world view.
    public void PassInTheWorld(World theWorld)
    {
        X = theWorld.WorldX;
        Z = theWorld.WorldZ;
        currentWorld = theWorld;
        ocean = GameObject.Find("WaterBasicDaytime");
        CreateTerrainObjects();
    }


    // Based on video by quil18 at https://www.youtube.com/watch?v=U9-1Gr5CLgk
    private void CreateTerrainObjects()
    {
        // Create the empty obects that are needed.
        Debug.Log("Running Create TerrainObjects");
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
        terrain.terrainData = terrainData;
        // Create the ocean
        CreateOcean(terrainData);
        // Paint the soils
        PaintSoils(terrain, terrainData);
        // Smooth out the squares???
        // ???????????
        // Paint the cliffs
        PaintRocks(terrainData);
        // Connect the terrain data to the terrain object
        terrain.terrainData = terrainData;
        tCollide.terrainData = terrainData;
        // Add the trees
        AddTrees(terrain, terrainData);
    }


    // Paint the rocks texture in appropriately
    // Maybe also add rock detail thingies
    private void PaintRocks(TerrainData terrainData)
    {
        float[,,] splatMaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        for (int aX = 0; aX < terrainData.alphamapWidth; aX++)
        {
            for (int aZ = 0; aZ < terrainData.alphamapHeight; aZ++)
            {
                float x = (float) aX / terrainData.alphamapWidth;
                float z = (float) aZ / terrainData.alphamapHeight;
                float angle = terrainData.GetSteepness(z, x); // Flip x and y
                float cliffiness = angle / 90f; 
                // Ulitmately normal will be the even terrain and cliffy will be the odd terrain.
                for (int i = 0; i < splatTextures.Length - 1; i += 2)
                {
                    splatMaps[aX, aZ, i] = (1f - cliffiness) * splatMaps[aX, aZ, i];
                    splatMaps[aX, aZ, i + 1] = cliffiness * splatMaps[aX, aZ, i];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatMaps);
    }


    // Paint the appropriate soils for the habitat type
    private void PaintSoils(Terrain currentTerrain, TerrainData terrainData)
    {
        Debug.Log(terrainData.alphamapWidth + ", " + terrainData.alphamapHeight);
        float[,,] splatMaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        for (int aX = 0; aX < terrainData.alphamapWidth; aX++)
        {
            for (int aZ = 0; aZ < terrainData.alphamapHeight; aZ++)
            {
                // Get the world coordinates
                int x = (int) (((double)aX / terrainData.alphamapWidth) * X);
                int z = (int) (((double)aZ / terrainData.alphamapHeight) * Z);
                // Base the land off the actual grid thing
                float presentHeight = currentTerrain.SampleHeight(new Vector3(z * SQUARE_MULTIPLIER, 0f, x * SQUARE_MULTIPLIER));
                if (((presentHeight / terrainData.size.y) * maxVertDist) < -minMaxElevationValues[0])
                {
                    splatMaps[aX, aZ, 0] = 0f;
                    splatMaps[aX, aZ, 2] = 1f;
                }
                // Else Paint the land habitat appropriate colors.
                else
                {
                    float[] soilPercents = CalculateSoilTypes(currentWorld.habitats.worldArray[x, z].typePercents);
                    for (int i = 0; i < splatTextures.Length; i += 2)
                    {
                        splatMaps[aX, aZ, i] = soilPercents[i / 2];
                    } 
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatMaps);
    }


    // Add the tree Prototypes
    private void LoadTreePrototypes(TerrainData terrainData)
    {
        TreePrototype[] treeProtos = new TreePrototype[3];
        for (int i = 0; i < treeModels.Length; i++)
        {
            treeProtos[i] = new TreePrototype();
            treeProtos[i].prefab = treeModels[i];
        }
        terrainData.treePrototypes = treeProtos;
    }


    // Populate the Trees
    private void AddTrees(Terrain currentTerrain, TerrainData terrainData)
    {
        List<TreeInstance> trees = new List<TreeInstance>();
        int p = 1;
        if (treePositions == null)
        {
            treePositions = new float[100];
            for (int t = 0; t < 100; t++)
            {
                treePositions[t] = Random.Range(0f, 100f);
            }
        }
        // Debug.Log(minMaxElevationValues[0]);
        for (int x = 0; x < X; x++)
        {
            for (int z = 0; z < Z; z++)
            {
                float[] treePercents = CalculateTreePercents(currentWorld.habitats.worldArray[x, z].typePercents);
                for (int t = 0; t < treePositions.Length; t++)
                {
                    float randy = Random.Range(0f, 1f);
                    float Lx = treePositions[t] / 10f;
                    float Lz = treePositions[(t + p) % 100] / 10f;
                    for (int i = 0; i < treeModels.Length; i++)
                    {
                        // For some reason seem to need to flip the x, z coordinates here
                        float presentHeight = currentTerrain.SampleHeight(new Vector3((z + .1f * Lz) * SQUARE_MULTIPLIER, 0f, (x + .1f * Lx) * SQUARE_MULTIPLIER));
                        if ((presentHeight / terrainData.size.y) * maxVertDist > -minMaxElevationValues[0] && randy < treePercents[i])
                        {
                            TreeInstance nextTree = new TreeInstance();
                            nextTree.prototypeIndex = i;
                            nextTree.heightScale = 1f;
                            nextTree.widthScale = 1f;
                            nextTree.position = new Vector3((z + .1f * Lz) / Z, presentHeight / terrainData.size.y, (x + .1f * Lx) / X);
                            nextTree.lightmapColor = Color.white;
                            trees.Add(nextTree);
                            p++;
                        }
                    }
                  
                } 
            }
        }
        terrainData.treeInstances = trees.ToArray();
    }


    // Populates the textures onto their respective objects
    private void BuildSplats(TerrainData terrainData)
    {
        SplatPrototype[] splatProtos = new SplatPrototype[splatTextures.Length];
        for (int i = 0; i < splatTextures.Length; i++)
        {
            splatProtos[i] = new SplatPrototype();
            splatProtos[i].texture = splatTextures[i];
            splatProtos[i].tileSize = new Vector2(30f, 30f);
        }
        terrainData.splatPrototypes = splatProtos;
    }


    private void ApplyModel(TerrainData terrainData)
    {
        terrainData.heightmapResolution = 64 + 1;
        terrainData.baseMapResolution = 32 + 1;
        terrainData.SetDetailResolution(64, 32);
        terrainData.alphamapResolution = 1000;
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


    private float[] CalculateSoilTypes(double[] habitatPercents)
    {
        float[] soilPercents = new float[(splatTextures.Length + 1) / 2];
        // desert soils - temperate to warm deserts
        soilPercents[1] = (float) (habitatPercents[5] + habitatPercents[9]);
        // mud soilds - swamps, tundra, rainforest
        soilPercents[2] = (float) (habitatPercents[1] + habitatPercents[2] + habitatPercents[4] + habitatPercents[8] + habitatPercents[12]);
        // glacier soils - glacier, maybe snow cover later
        soilPercents[3] = (float) habitatPercents[0];
        // normal soils - grasslands, forests
        soilPercents[0] = 1.0f - soilPercents[1] - soilPercents[2] - soilPercents[3];

        return soilPercents;
    }


    private float[] CalculateTreePercents(double[] habitatPercents)
    {
        float[] treePercents = new float[3];
        // Conifers are cold trees - in future maybe artic marsh is marsh plants???
        treePercents[0] = (float) (habitatPercents[3] + habitatPercents[4]);
        // Decidious Trees are temperate trees - in future maybe swamp is marsh plants???
        treePercents[1] = (float)(habitatPercents[7] + habitatPercents[8]);
        // Palm are tropical trees
        treePercents[2] = (float)(habitatPercents[11] + habitatPercents[12]);

        return treePercents;
    }


    // Create the ocean
    private void CreateOcean(TerrainData terrainData)
    {
        ocean.transform.position = new Vector3((X * SQUARE_MULTIPLIER / 2.0f), (-minMaxElevationValues[0] / maxVertDist) * terrainData.size.y, Z * SQUARE_MULTIPLIER / 2.0f);
        ocean.transform.localScale = new Vector3(X, 0f, Z);
    }

}