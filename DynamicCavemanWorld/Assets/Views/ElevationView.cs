using UnityEngine;
using System.Collections;

public class ElevationView : MonoBehaviour {

    public static Mesh BuildMesh(SingleValueLayer elevationLayer, MeshFilter meshFilter, MeshCollider meshCollider, MeshRenderer meshRenderer)
    {
        // Set some constants
        int numOfTilesX = SingleValueLayer.WORLDX;
        int numOfTilesZ = SingleValueLayer.WORLDZ;
        int numOfTiles = numOfTilesX * numOfTilesZ;
        int numVertices = (numOfTilesX + 1) * (numOfTilesZ + 1);
        float tileSize = 5.0f;
        
        // Convert to mesh data
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[2 * numOfTilesZ * 3];
        Vector3[] normals = new Vector3[numVertices];

        // Create the vertices and the normals generically
        int x, z;
        for (z = 0; z < numOfTilesZ + 1; z++)
        {
            for (x = 0; x < numOfTilesX + 1; x++)
            {
                vertices[z * (numOfTilesX + 1) + x] = new Vector3(x * tileSize, 0, z * tileSize);
                normals[z * (numOfTilesX + 1) + x] = Vector3.up;
                // uv[z * (numOfTilesX + 1) + x] = new Vector2((float) x / (numOfTilesX + 1), (float) z / (numOfTilesZ + 1));
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

}
